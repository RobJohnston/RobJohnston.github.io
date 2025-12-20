+++
title = "Lean Interfaces: Don't Force Scientists to Implement What They Don't Need"
date = 2026-01-26
publishDate = 2026-01-26
draft = true
showAuthor = false
sharingLinks = true
categories = [
    "SOLID Principles",
]
tags = [
    "Interface Segregation Principle",
]
image = ""
description = ""
toc = true
+++

*Part 4 of the SOLID Principles for Scientific Programmers series*

## The Interface Bloat Problem

You're building a data processing library for your research group. You create a `DataSource` interface that defines everything a data source might need to do: read data, write data, seek to positions, get metadata, validate checksums, compress, encrypt, and more.

Then someone wants to use your library with a simple CSV file reader. Now they have to implement *all* those methods, even though CSV files don't support seeking, compression, or encryption. So they write dummy methods that throw "not supported" errors or just return `None`.

Later, someone writes code that assumes all `DataSource` objects support compression. It compiles fine, runs fine with some data sources, then crashes with the CSV reader because that feature isn't really implemented.

Your well-intentioned interface has become a burden rather than a help.

This is the problem the **Interface Segregation Principle (ISP)** solves.

## What Is the Interface Segregation Principle?

Robert C. Martin stated it as:

> **No client should be forced to depend on methods it does not use.**

In practical terms for scientists: **Break large interfaces into smaller, focused ones. Classes should only implement the interfaces they actually need, not be forced to implement dummy methods for features they don't support.**

Think of it like equipment capabilities: Not every instrument needs every feature. Your basic pH meter doesn't need the same interface as your automated high-throughput system. They should implement different interfaces based on their actual capabilities.

## A Real Example: The Problem

Let's look at a data source abstraction that forces everything into one large interface:

```python
from abc import ABC, abstractmethod
import numpy as np
from typing import Dict, Any, Optional
import json

class DataSource(ABC):
    """
    Master interface for all data sources.
    Every data source must implement ALL of these methods!
    """

    @abstractmethod
    def read(self) -> np.ndarray:
        """Read data from source."""
        pass

    @abstractmethod
    def write(self, data: np.ndarray) -> None:
        """Write data to source."""
        pass

    @abstractmethod
    def append(self, data: np.ndarray) -> None:
        """Append data to existing source."""
        pass

    @abstractmethod
    def seek(self, position: int) -> None:
        """Seek to specific position in data stream."""
        pass

    @abstractmethod
    def get_metadata(self) -> Dict[str, Any]:
        """Get metadata about the data source."""
        pass

    @abstractmethod
    def set_metadata(self, metadata: Dict[str, Any]) -> None:
        """Set metadata for the data source."""
        pass

    @abstractmethod
    def validate_checksum(self) -> bool:
        """Validate data integrity via checksum."""
        pass

    @abstractmethod
    def compress(self) -> None:
        """Compress the data source."""
        pass

    @abstractmethod
    def decompress(self) -> None:
        """Decompress the data source."""
        pass

    @abstractmethod
    def encrypt(self, key: str) -> None:
        """Encrypt the data source."""
        pass

    @abstractmethod
    def get_size(self) -> int:
        """Get size of data in bytes."""
        pass

    @abstractmethod
    def close(self) -> None:
        """Close the data source."""
        pass

# Simple CSV file - forced to implement everything!
class CSVDataSource(DataSource):
    """Read data from CSV file."""

    def __init__(self, filename: str):
        self.filename = filename
        self.data = None

    def read(self) -> np.ndarray:
        """Actually works - CSV files can be read."""
        import pandas as pd
        df = pd.read_csv(self.filename)
        return df.values

    def write(self, data: np.ndarray) -> None:
        """CSV files are read-only in our setup."""
        raise NotImplementedError("CSV source is read-only")

    def append(self, data: np.ndarray) -> None:
        """Can't append to CSV."""
        raise NotImplementedError("CSV source doesn't support append")

    def seek(self, position: int) -> None:
        """CSV files don't support seeking."""
        raise NotImplementedError("CSV source doesn't support seeking")

    def get_metadata(self) -> Dict[str, Any]:
        """CSV has no metadata format."""
        return {}  # Return empty dict

    def set_metadata(self, metadata: Dict[str, Any]) -> None:
        """Can't store metadata in CSV."""
        raise NotImplementedError("CSV doesn't support metadata")

    def validate_checksum(self) -> bool:
        """CSV files have no checksum."""
        return True  # Just pretend it's valid

    def compress(self) -> None:
        """CSV compression not supported."""
        raise NotImplementedError("CSV compression not supported")

    def decompress(self) -> None:
        """CSV decompression not supported."""
        raise NotImplementedError("CSV decompression not supported")

    def encrypt(self, key: str) -> None:
        """CSV encryption not supported."""
        raise NotImplementedError("CSV encryption not supported")

    def get_size(self) -> int:
        """Can implement this one."""
        import os
        return os.path.getsize(self.filename)

    def close(self) -> None:
        """Nothing to close for CSV."""
        pass  # Do nothing

# Database source - also forced to implement everything!
class DatabaseDataSource(DataSource):
    """Read data from database."""

    def __init__(self, connection_string: str, table: str):
        self.connection_string = connection_string
        self.table = table

    def read(self) -> np.ndarray:
        """Can read from database."""
        # Simulate database read
        return np.random.rand(100, 10)

    def write(self, data: np.ndarray) -> None:
        """Can write to database."""
        # Implementation here
        pass

    def append(self, data: np.ndarray) -> None:
        """Can append to database."""
        # Implementation here
        pass

    def seek(self, position: int) -> None:
        """Databases don't work like files - no seeking."""
        raise NotImplementedError("Database doesn't support seeking")

    def get_metadata(self) -> Dict[str, Any]:
        """Can get metadata from database."""
        return {'table': self.table, 'rows': 100}

    def set_metadata(self, metadata: Dict[str, Any]) -> None:
        """Databases don't store arbitrary metadata this way."""
        raise NotImplementedError("Database metadata is schema-defined")

    def validate_checksum(self) -> bool:
        """Databases have their own integrity checking."""
        raise NotImplementedError("Use database DBCC instead")

    def compress(self) -> None:
        """Database compression is at server level."""
        raise NotImplementedError("Database compression not available")

    def decompress(self) -> None:
        """Database decompression is at server level."""
        raise NotImplementedError("Database decompression not available")

    def encrypt(self, key: str) -> None:
        """Database encryption is at server level."""
        raise NotImplementedError("Database encryption not available")

    def get_size(self) -> int:
        """Can query size from database."""
        # Implementation here
        return 1024

    def close(self) -> None:
        """Must close database connection."""
        # Close connection
        pass

# Now code that uses these sources has problems
def process_with_compression(source: DataSource):
    """Process data with compression for efficiency."""
    data = source.read()
    source.compress()  # Might crash if not supported!
    # Process data
    return data

def backup_data(source: DataSource, backup_source: DataSource):
    """Backup data from one source to another."""
    data = source.read()
    backup_source.write(data)  # Crashes if backup is read-only!

# Usage - lots of runtime errors!
csv = CSVDataSource("data.csv")
process_with_compression(csv)  # Crashes - CSV doesn't compress!

csv2 = CSVDataSource("backup.csv")
backup_data(csv, csv2)  # Crashes - CSV is read-only!
```

## Problems with This Design

### Problem 1: Forced to Implement Irrelevant Methods

The CSV reader has to implement 9 methods it doesn't support, filling them with `NotImplementedError` or dummy returns.

### Problem 2: Runtime Errors Instead of Compile-Time Safety

Code compiles fine even when using unsupported features. Errors only appear at runtime.

### Problem 3: Misleading Interface

The interface promises features (compression, encryption) that many implementations don't actually provide.

### Problem 4: Difficult to Understand

Looking at `CSVDataSource`, which methods actually work? You have to read the implementation to find out.

### Problem 5: Fragile Code

Code written against the interface can't rely on anything working. Every method call is a potential runtime error.

## The Solution: Interface Segregation Principle

Let's break this monolithic interface into focused, cohesive interfaces:

```python
from abc import ABC, abstractmethod
import numpy as np
from typing import Dict, Any, Protocol
import pandas as pd

# CORE INTERFACE: Minimum that all data sources provide
class Readable(ABC):
    """Data sources that can be read."""

    @abstractmethod
    def read(self) -> np.ndarray:
        """Read data from source."""
        pass

class Writeable(ABC):
    """Data sources that can be written to."""

    @abstractmethod
    def write(self, data: np.ndarray) -> None:
        """Write data to source."""
        pass

class Appendable(ABC):
    """Data sources that support appending."""

    @abstractmethod
    def append(self, data: np.ndarray) -> None:
        """Append data to existing source."""
        pass

class Seekable(ABC):
    """Data sources that support seeking."""

    @abstractmethod
    def seek(self, position: int) -> None:
        """Seek to specific position."""
        pass

    @abstractmethod
    def tell(self) -> int:
        """Return current position."""
        pass

class HasMetadata(ABC):
    """Data sources with metadata support."""

    @abstractmethod
    def get_metadata(self) -> Dict[str, Any]:
        """Get metadata."""
        pass

    @abstractmethod
    def set_metadata(self, metadata: Dict[str, Any]) -> None:
        """Set metadata."""
        pass

class Compressible(ABC):
    """Data sources that support compression."""

    @abstractmethod
    def compress(self) -> None:
        """Compress the data."""
        pass

    @abstractmethod
    def decompress(self) -> None:
        """Decompress the data."""
        pass

    @abstractmethod
    def is_compressed(self) -> bool:
        """Check if data is compressed."""
        pass

class Encryptable(ABC):
    """Data sources that support encryption."""

    @abstractmethod
    def encrypt(self, key: str) -> None:
        """Encrypt the data."""
        pass

    @abstractmethod
    def decrypt(self, key: str) -> None:
        """Decrypt the data."""
        pass

    @abstractmethod
    def is_encrypted(self) -> bool:
        """Check if data is encrypted."""
        pass

class Validatable(ABC):
    """Data sources that support integrity checking."""

    @abstractmethod
    def validate_checksum(self) -> bool:
        """Validate data integrity."""
        pass

    @abstractmethod
    def compute_checksum(self) -> str:
        """Compute checksum of data."""
        pass

class Sizeable(ABC):
    """Data sources with queryable size."""

    @abstractmethod
    def get_size(self) -> int:
        """Get size in bytes."""
        pass

class Closeable(ABC):
    """Resources that need cleanup."""

    @abstractmethod
    def close(self) -> None:
        """Close and cleanup resources."""
        pass

# Now implementations only implement what they support!

class CSVDataSource(Readable, Sizeable):
    """
    CSV file reader - only implements what it supports.
    Readable: Yes, can read CSV files
    Sizeable: Yes, can get file size
    """

    def __init__(self, filename: str):
        self.filename = filename

    def read(self) -> np.ndarray:
        """Read CSV file."""
        df = pd.read_csv(self.filename)
        return df.values

    def get_size(self) -> int:
        """Get file size."""
        import os
        return os.path.getsize(self.filename)

class DatabaseSource(Readable, Writable, Appendable, HasMetadata, Sizeable, Closeable):
    """
    Database source - implements many interfaces.
    Can read, write, append, has metadata, has size, needs closing.
    """

    def __init__(self, connection_string: str, table: str):
        self.connection_string = connection_string
        self.table = table
        self.connection = None

    def read(self) -> np.ndarray:
        """Read from database."""
        # Implementation
        return np.random.rand(100, 10)

    def write(self, data: np.ndarray) -> None:
        """Write to database."""
        # Implementation
        pass

    def append(self, data: np.ndarray) -> None:
        """Append to database."""
        # Implementation
        pass

    def get_metadata(self) -> Dict[str, Any]:
        """Get table metadata."""
        return {'table': self.table, 'rows': 100}

    def set_metadata(self, metadata: Dict[str, Any]) -> None:
        """Update table metadata."""
        # Implementation
        pass

    def get_size(self) -> int:
        """Get table size."""
        return 1024

    def close(self) -> None:
        """Close database connection."""
        if self.connection:
            self.connection.close()

class BinaryFileSource(Readable, Writable, Seekable, Compressible, Sizeable, Closeable):
    """
    Binary file with full features.
    Supports reading, writing, seeking, compression.
    """

    def __init__(self, filename: str):
        self.filename = filename
        self.file = None
        self.compressed = False

    def read(self) -> np.ndarray:
        """Read binary data."""
        return np.load(self.filename)

    def write(self, data: np.ndarray) -> None:
        """Write binary data."""
        np.save(self.filename, data)

    def seek(self, position: int) -> None:
        """Seek in file."""
        if self.file:
            self.file.seek(position)

    def tell(self) -> int:
        """Get current position."""
        return self.file.tell() if self.file else 0

    def compress(self) -> None:
        """Compress file."""
        import gzip
        # Compression implementation
        self.compressed = True

    def decompress(self) -> None:
        """Decompress file."""
        # Decompression implementation
        self.compressed = False

    def is_compressed(self) -> bool:
        """Check compression status."""
        return self.compressed

    def get_size(self) -> int:
        """Get file size."""
        import os
        return os.path.getsize(self.filename)

    def close(self) -> None:
        """Close file."""
        if self.file:
            self.file.close()

class InMemorySource(Readable, Writable):
    """
    Simple in-memory data source.
    Just readable and writable - nothing fancy.
    """

    def __init__(self):
        self.data = None

    def read(self) -> np.ndarray:
        """Read from memory."""
        return self.data

    def write(self, data: np.ndarray) -> None:
        """Write to memory."""
        self.data = data.copy()

# Now functions specify exactly what they need!

def read_data(source: Readable) -> np.ndarray:
    """Read data from any readable source."""
    return source.read()

def backup_data(source: Readable, destination: Writable) -> None:
    """Backup data from readable to writable source."""
    data = source.read()
    destination.write(data)

def compress_if_possible(source: Readable) -> None:
    """Compress source if it supports compression."""
    if isinstance(source, Compressible):
        source.compress()
        print("Data compressed")
    else:
        print("Source doesn't support compression - skipping")

def process_with_metadata(source: Readable & HasMetadata) -> Dict[str, Any]:
    """Process data and return metadata (requires both interfaces)."""
    data = source.read()
    metadata = source.get_metadata()
    # Process data
    metadata['processed'] = True
    return metadata

def safe_close(source: Any) -> None:
    """Close source if it needs closing."""
    if isinstance(source, Closeable):
        source.close()

# Usage - type-safe and clear!

# CSV reading works
csv = CSVDataSource("data.csv")
data = read_data(csv)  # Works - CSV is Readable

# Compression is optional
compress_if_possible(csv)  # Prints "doesn't support compression"
binary = BinaryFileSource("data.npy")
compress_if_possible(binary)  # Actually compresses

# Backup works with any combination
db = DatabaseSource("connection", "table")
backup_data(csv, db)  # CSV (Readable) → Database (Writable) ✓
backup_data(db, binary)  # Database → Binary ✓

# Compile-time safety with type hints
def needs_writable(dest: Writable) -> None:
    dest.write(np.array([1, 2, 3]))

# needs_writable(csv)  # Type checker catches this! CSV isn't Writable

# Metadata only works with sources that have it
# metadata = process_with_metadata(csv)  # Type error - CSV doesn't have HasMetadata
metadata = process_with_metadata(db)  # Works - Database has HasMetadata

# Cleanup is explicit
safe_close(csv)  # Does nothing - CSV doesn't need closing
safe_close(db)  # Closes database connection
```

## Why This Is Better

### 1. Implement Only What You Support

`CSVDataSource` implements 2 interfaces instead of 12 methods. No dummy implementations!

### 2. Type Safety

Functions declare exactly what they need:

```python
def backup_data(source: Readable, dest: Writable)
```

Type checkers verify you're passing compatible sources at compile time.

### 3. Clear Capabilities

Looking at class declaration tells you exactly what it supports:

```python
class BinaryFileSource(Readable, Writable, Seekable, Compressible, ...)
```

### 4. Flexible Combinations

Different sources implement different combinations of interfaces based on their actual capabilities.

### 5. Graceful Feature Detection

```python
if isinstance(source, Compressible):
    source.compress()
```

Code can check for optional features without causing errors.

## Python-Specific: Protocols vs ABCs

Python 3.8+ added Protocols for structural typing (duck typing with type hints):

```python
from typing import Protocol

class Readable(Protocol):
    """Readable as a Protocol - no inheritance needed!"""

    def read(self) -> np.ndarray:
        """Read data."""
        ...

# Any class with a read() method is automatically Readable!
class MyCustomSource:
    def read(self) -> np.ndarray:
        return np.array([1, 2, 3])

def process(source: Readable):
    """Works with anything that has read()."""
    return source.read()

# No inheritance needed!
custom = MyCustomSource()
process(custom)  # Type checker is happy
```

Use **Protocols** when:

- You want duck typing with type checking
- Third-party classes can't inherit from your interfaces
- You want minimal coupling

Use **ABCs** when:

- You want to enforce implementation
- You want to provide shared method implementations
- You want explicit interface contracts

## Real-World Example: Instrument Interface

Here's how ISP applies to laboratory instruments:

```python
# BAD: One giant instrument interface
class Instrument(ABC):
    @abstractmethod
    def initialize(self): pass

    @abstractmethod
    def measure(self): pass

    @abstractmethod
    def calibrate(self): pass

    @abstractmethod
    def get_temperature(self): pass  # Not all instruments measure temperature!

    @abstractmethod
    def get_pressure(self): pass  # Not all measure pressure!

    @abstractmethod
    def set_wavelength(self, wl): pass  # Only spectroscopy!

    @abstractmethod
    def start_scan(self): pass  # Only scanning instruments!

# GOOD: Segregated interfaces
class Instrument(ABC):
    """Base interface - what ALL instruments have."""
    @abstractmethod
    def initialize(self): pass

    @abstractmethod
    def shutdown(self): pass

class Measurable(ABC):
    """Instruments that take measurements."""
    @abstractmethod
    def measure(self) -> Measurement: pass

class Calibratable(ABC):
    """Instruments that support calibration."""
    @abstractmethod
    def calibrate(self, reference: Any): pass

class TemperatureSensor(ABC):
    """Instruments that measure temperature."""
    @abstractmethod
    def get_temperature(self) -> float: pass

class Spectrometer(ABC):
    """Spectroscopy-specific features."""
    @abstractmethod
    def set_wavelength(self, wavelength: float): pass

    @abstractmethod
    def get_spectrum(self) -> np.ndarray: pass

class Scanner(ABC):
    """Scanning instruments."""
    @abstractmethod
    def start_scan(self): pass

    @abstractmethod
    def stop_scan(self): pass

# Now implementations are clean
class UVVisSpectrometer(Instrument, Spectrometer, Measurable, Calibratable):
    """UV-Vis spectrometer with all its actual capabilities."""
    pass

class SimpleThermocouple(Instrument, TemperatureSensor):
    """Simple thermocouple - just temperature, no calibration."""
    pass

class ScanningTunnelMicroscope(Instrument, Scanner, Measurable):
    """STM - scanning and measuring, but not a spectrometer."""
    pass
```

## The Role Interface vs Interface Segregation

Sometimes you need role-based interfaces that group related functionality:

```python
# ROLE: Data analysis pipeline component
class PipelineStage(Readable, Writable):
    """A stage in a pipeline must read input and write output."""
    pass

# ROLE: Archivable data
class Archivable(Readable, Compressible, Validatable):
    """Data that can be archived needs these three capabilities."""
    pass

# Specific implementation combines roles
class AnalysisResult(PipelineStage, Archivable, HasMetadata):
    """Analysis results play multiple roles."""
    pass
```

This is fine! Role interfaces compose smaller interfaces. The key: don't force implementation of methods that aren't needed.

## Testing with Segregated Interfaces

Small interfaces make testing easier:

```python
import unittest
from unittest.mock import Mock

class TestDataProcessing(unittest.TestCase):
    def test_read_data(self):
        """Test with minimal mock - only need read() method."""
        mock_source = Mock(spec=Readable)
        mock_source.read.return_value = np.array([1, 2, 3])

        result = read_data(mock_source)

        self.assertEqual(len(result), 3)
        mock_source.read.assert_called_once()

    def test_backup_data(self):
        """Test with two mocks - one readable, one writable."""
        mock_source = Mock(spec=Readable)
        mock_source.read.return_value = np.array([1, 2, 3])

        mock_dest = Mock(spec=Writable)

        backup_data(mock_source, mock_dest)

        mock_source.read.assert_called_once()
        mock_dest.write.assert_called_once()

# With fat interface, would need to mock 12 methods!
# With ISP, only mock what you need!
```

## Common Mistakes

### Mistake 1: Too Many Small Interfaces

Don't create an interface for every single method:

```python
# TOO GRANULAR
class CanGetX(ABC):
    @abstractmethod
    def get_x(self): pass

class CanGetY(ABC):
    @abstractmethod
    def get_y(self): pass

# BETTER: Group related operations
class Point2D(ABC):
    @abstractmethod
    def get_x(self): pass

    @abstractmethod
    def get_y(self): pass
```

### Mistake 2: Interfaces That Are Still Too Large

```python
# STILL TOO LARGE
class DataProcessing(ABC):
    @abstractmethod
    def load(self): pass

    @abstractmethod
    def clean(self): pass

    @abstractmethod
    def transform(self): pass

    @abstractmethod
    def analyze(self): pass

    @abstractmethod
    def visualize(self): pass

# BETTER: Split by client needs
class DataLoader(ABC):
    @abstractmethod
    def load(self): pass

class DataCleaner(ABC):
    @abstractmethod
    def clean(self, data): pass

class DataAnalyzer(ABC):
    @abstractmethod
    def analyze(self, data): pass
```

## When to Segregate Interfaces

**Segregate when:**

- ✅ Different clients need different subsets of functionality
- ✅ Implementations often don't support all features
- ✅ You find yourself writing many `raise NotImplementedError`
- ✅ You have optional features that not all implementations provide

**Don't segregate when:**

- ✅ All implementations genuinely need all methods
- ✅ The methods are tightly coupled (can't use one without the others)
- ✅ You're creating an internal class, not a public interface
- ✅ Segregation would create more confusion than clarity

## The Client Perspective

ISP is about clients (code that uses interfaces), not implementations:

```python
# This function only needs reading
def analyze_data(source: Readable):
    data = source.read()
    return np.mean(data)

# Works with ANY readable source!
analyze_data(csv)
analyze_data(db)
analyze_data(binary)
analyze_data(memory)

# Doesn't care about compression, encryption, metadata, etc.
```

## Summary

The Interface Segregation Principle says: **No client should be forced to depend on methods it does not use.**

Following ISP in scientific code:

- Reduces dummy implementations
- Improves type safety
- Makes capabilities explicit
- Enables flexible combinations
- Simplifies testing
- Creates more maintainable code

**The key insight**: Design interfaces from the client's perspective, not the implementer's. Ask "What does this client need?" not "What can this implementation do?"

## Practical Guidelines

**Before creating an interface, ask:**

1. Will all implementations support all methods?
2. Do all clients need all methods?
3. Are there natural groupings of methods?
4. Can I split this into smaller, focused interfaces?

**If you find yourself:**

- Writing `raise NotImplementedError` frequently
- Implementing dummy methods
- Checking `isinstance` before calling methods
- **→ Your interface needs segregation**

## Next Steps

1. Find a large interface in your code
2. List which implementations support which methods
3. Group methods by which clients use them together
4. Split into smaller interfaces
5. Update implementations to only implement what they support

In the final post of this series, we'll bring together all five SOLID principles and discuss when to apply them (and when not to) in real scientific programming scenarios.

---

*Have you written `raise NotImplementedError` more times than you'd like? Share your interface horror stories in the comments!*

**Previous posts in this series:**

- [Single Responsibility Principle for Scientists](srp_blog_post.md)
- [Open/Closed Principle for Scientists](ocp_blog_post.md)
- [Liskov Substitution Principle for Scientists](lsp_blog_post.md)

**Next in this series:**

- [The Dependency Inversion Principle for Scientists](dip_blog_post.md) - Coming next week

**Related posts:**

- Duck Typing vs. Explicit Interfaces
- Protocol Classes in Python 3.8+
- Designing Composable Interfaces for Laboratory Software
