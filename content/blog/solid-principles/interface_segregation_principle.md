+++
title = "Lean Interfaces: Why Would a pH Meter Need `set_wavelength()`?"
date = 2026-01-21T07:42:06-05:00
publishDate = "2026-01-26"
draft = true
description = "Why does a pH meter need set_wavelength()? Learn Interface Segregation Principle to stop forcing instruments to implement methods they don't support."
category = "SOLID Principles"
tags = ["ISP"]
image = "/images/solid/interface_segregation.png"
+++

*Part 4 of the SOLID Principles for Scientific Programmers series*

## The Interface Bloat Problem

You're writing software to control laboratory instruments in your research group. You create an `Instrument` interface that defines everything an instrument might need to do: initialize, shutdown, measure, calibrate, read temperature, read pressure, set wavelength, start scanning, and more.

Then someone wants to use your library with a simple thermocouple. Now they have to implement *all* those methods—10 in total. Several of them throw 'not implemented' errors. Your well-meaning abstraction has become a minefield where every method call might explode at runtime. Your colleague's automated experiment crashes at 2 AM during data collection because the code tried to call `get_pressure()` on a thermocouple that only measures temperature, and nobody knew this would fail until production.

Later, someone writes code that assumes all `Instrument` objects support wavelength control. It compiles fine, runs fine with the UV-Vis spectrometer, then crashes with the thermocouple because that feature isn't really implemented.

Your well-intentioned interface has become a burden rather than a help.

This is the problem the **Interface Segregation Principle (ISP)** solves.

## What Is the Interface Segregation Principle?

Robert C. Martin stated it as:

> **No client should be forced to depend on methods it does not use.**  [^1]

[^1]: *Martin, Robert C.*, [Agile Software Development: Principles, Patterns, and Practices](https://amzn.to/49mEjmD) (paid link). Pearson Education, 2003.

In practical terms for scientists: **Break large interfaces into smaller, focused ones. Classes should only implement the interfaces they actually need, not be forced to implement dummy methods for features they don't support.**

Think of it like equipment capabilities: Not every instrument needs every feature. Your basic pH meter doesn't need the same interface as your automated high-throughput system. They should implement different interfaces based on their actual capabilities.

## Before You Refactor: Is It Worth It?

Interface bloat often appears gradually as requirements accumulate. Before refactoring, consider:

- **Are implementations writing many `raise NotImplementedError`?** This is the clearest ISP violation signal
- **Do clients need different subsets of functionality?** If every client uses every method, one interface is fine
- **Is this a public interface?** Internal classes used by one client don't need segregation
- **Are you seeing `isinstance` checks before method calls?** This suggests the interface promises features some implementations don't provide

If implementations are littered with dummy methods or not-implemented errors, ISP refactoring is essential.

## A Real Example: The Problem

Let's look at an instrument abstraction that forces everything into one large interface:

```python
from abc import ABC, abstractmethod
import numpy as np
from typing import Any

class Instrument(ABC):
    """
    Master interface for all laboratory instruments.
    Every instrument must implement ALL of these methods!
    """

    @abstractmethod
    def initialize(self) -> None:
        """Initialize the instrument."""
        pass

    @abstractmethod
    def shutdown(self) -> None:
        """Shutdown the instrument."""
        pass

    @abstractmethod
    def measure(self) -> float:
        """Take a measurement."""
        pass

    @abstractmethod
    def calibrate(self, reference: Any) -> None:
        """Calibrate against a reference."""
        pass

    @abstractmethod
    def get_temperature(self) -> float:
        """Read temperature."""
        pass

    @abstractmethod
    def get_pressure(self) -> float:
        """Read pressure."""
        pass

    @abstractmethod
    def set_wavelength(self, wavelength: float) -> None:
        """Set wavelength for optical measurements."""
        pass

    @abstractmethod
    def get_spectrum(self) -> np.ndarray:
        """Acquire full spectrum."""
        pass

    @abstractmethod
    def start_scan(self) -> None:
        """Start a scanning operation."""
        pass

    @abstractmethod
    def stop_scan(self) -> None:
        """Stop scanning."""
        pass

# Simple thermocouple - forced to implement everything!
class Thermocouple(Instrument):
    """Simple temperature sensor."""

    def __init__(self, channel: int):
        self.channel = channel
        self.temperature = 25.0

    def initialize(self) -> None:
        """Actually works."""
        print(f"Thermocouple on channel {self.channel} initialized")

    def shutdown(self) -> None:
        """Actually works."""
        print("Thermocouple shutdown")

    def measure(self) -> float:
        """Returns temperature."""
        return self.get_temperature()

    def calibrate(self, reference: Any) -> None:
        """Thermocouples can be calibrated."""
        print(f"Calibrating against {reference}°C")

    def get_temperature(self) -> float:
        """Actually works - this is what it does!"""
        return self.temperature + np.random.normal(0, 0.1)

    def get_pressure(self) -> float:
        """Thermocouples don't measure pressure!"""
        raise NotImplementedError("Thermocouple doesn't measure pressure")

    def set_wavelength(self, wavelength: float) -> None:
        """Thermocouples aren't optical!"""
        raise NotImplementedError("Thermocouple doesn't use wavelengths")

    def get_spectrum(self) -> np.ndarray:
        """Not a spectrometer!"""
        raise NotImplementedError("Thermocouple can't acquire spectra")

    def start_scan(self) -> None:
        """Not a scanning device!"""
        raise NotImplementedError("Thermocouple doesn't scan")

    def stop_scan(self) -> None:
        """Not a scanning device!"""
        raise NotImplementedError("Thermocouple doesn't scan")

# UV-Vis Spectrometer - also forced to implement everything!
class UVVisSpectrometer(Instrument):
    """UV-Visible spectrometer."""

    def __init__(self):
        self.wavelength = 550.0
        self.initialized = False

    def initialize(self) -> None:
        self.initialized = True

    def shutdown(self) -> None:
        self.initialized = False

    def measure(self) -> float:
        """Returns intensity at current wavelength."""
        return np.random.rand()

    def calibrate(self, reference: Any) -> None:
        """Can calibrate with reference sample."""
        print("Calibrating spectrometer")

    def get_temperature(self) -> float:
        """Spectrometers don't measure temperature!"""
        raise NotImplementedError("UV-Vis doesn't measure temperature")

    def get_pressure(self) -> float:
        """Not a pressure sensor!"""
        raise NotImplementedError("UV-Vis doesn't measure pressure")

    def set_wavelength(self, wavelength: float) -> None:
        """Actually works - this is what it does!"""
        self.wavelength = wavelength

    def get_spectrum(self) -> np.ndarray:
        """Actually works!"""
        wavelengths = np.linspace(200, 800, 100)
        intensities = np.random.rand(100)
        return np.column_stack([wavelengths, intensities])

    def start_scan(self) -> None:
        """Not a scanning instrument!"""
        raise NotImplementedError("UV-Vis doesn't scan")

    def stop_scan(self) -> None:
        raise NotImplementedError("UV-Vis doesn't scan")

# Code that uses instruments has problems
def read_temperature(instrument: Instrument) -> float:
    """Read temperature from any instrument."""
    return instrument.get_temperature()  # Crashes if not a thermometer!

def acquire_spectrum(instrument: Instrument) -> np.ndarray:
    """Acquire spectrum from any instrument."""
    instrument.set_wavelength(550.0)
    return instrument.get_spectrum()  # Crashes if not a spectrometer!

# Usage - runtime errors!
thermocouple = Thermocouple(1)
spectrum = acquire_spectrum(thermocouple)  # CRASH!

uvvis = UVVisSpectrometer()
temp = read_temperature(uvvis)  # CRASH!
```

## Problems with This Design

**Problem 1**: Forced to Implement Irrelevant Methods

The `Thermocouple` class has to implement 5 methods it doesn't support, filling them with `NotImplementedError` or dummy returns.

**Problem 2**: Runtime Errors Instead of Compile-Time Safety

Code compiles fine even when using unsupported features. Errors only appear at runtime.

**Problem 3**: Misleading Interface

The interface promises features (e.g., calibration) that many implementations don't actually provide.

**Problem 4**: Difficult to Understand

Looking at `UVVisSpectrometer`, which methods actually work? You have to read the implementation to find out.

**Problem 5**: Fragile Code

Code written against the interface can't rely on anything working. Every method call is a potential runtime error.

## The Solution: Interface Segregation Principle

**This solves the interface bloat problem**: instead of forcing all implementations to support all methods, break interfaces into focused capabilities. Implementations only implement what they actually support.

```ascii
BEFORE (fat interface):          AFTER (segregated):

┌──────────────┐                ┌────────────┐
│ Instrument   │                │ Instrument │ (base)
│ - initialize │                └────────────┘
│ - measure    │                ┌────────────┐ ┌─────────────┐
│ - calibrate  │                │ Measurable │ │Calibratable │
│ - get_temp   │                └────────────┘ └─────────────┘
│ - get_press  │                ┌──────────────────┐
│ - set_wl     │                │TemperatureSensor │
│ - spectrum   │                └──────────────────┘
│ - scan       │                ┌─────────────┐
│ (10 methods) │                │ Spectrometer│
└──────────────┘                └─────────────┘
       △
       │                        ┌─────────────────┐
  ┌────┴────┐                   │ Thermocouple    │
  │         │                   │ (Instrument +   │
┌──────┐ ┌──────┐               │  TemperatureSensor
│Thermo│ │UVVis │               │  + Calibratable)│
│ 6/10 │ │ 5/10 │               └─────────────────┘
│ NOT  │ │ NOT  │
│ IMPL │ │ IMPL │               ┌──────────────────┐
└──────┘ └──────┘               │ UVVisSpectrometer│
                                │ (Instrument +    │
                                │  Spectrometer +  │
                                │  Measurable +    │
                                │  Calibratable)   │
                                └──────────────────┘
```

```python
# CORE INTERFACES: Split by actual capabilities

class Instrument(ABC):
    """Base interface - what ALL instruments have."""
    @abstractmethod
    def initialize(self) -> None:
        pass

    @abstractmethod
    def shutdown(self) -> None:
        pass

class Measurable(ABC):
    """Instruments that take measurements."""
    @abstractmethod
    def measure(self) -> float:
        pass

class Calibratable(ABC):
    """Instruments that support calibration."""
    @abstractmethod
    def calibrate(self, reference: Any) -> None:
        pass

class TemperatureSensor(ABC):
    """Instruments that measure temperature."""
    @abstractmethod
    def get_temperature(self) -> float:
        pass

class PressureSensor(ABC):
    """Instruments that measure pressure."""
    @abstractmethod
    def get_pressure(self) -> float:
        pass

class Spectrometer(ABC):
    """Spectroscopy-specific features."""
    @abstractmethod
    def set_wavelength(self, wavelength: float) -> None:
        pass

    @abstractmethod
    def get_spectrum(self) -> np.ndarray:
        pass

class Scanner(ABC):
    """Scanning instruments."""
    @abstractmethod
    def start_scan(self) -> None:
        pass

    @abstractmethod
    def stop_scan(self) -> None:
        pass

```

```python
# CONCRETE IMPLEMENTATIONS: Only implement what they support!

class Thermocouple(Instrument, TemperatureSensor, Calibratable, Measurable):
    """Simple thermocouple - only temperature capabilities."""

    def __init__(self, channel: int):
        self.channel = channel
        self.temperature = 25.0

    def initialize(self) -> None:
        print(f"Thermocouple on channel {self.channel} initialized")

    def shutdown(self) -> None:
        print("Thermocouple shutdown")

    def measure(self) -> float:
        return self.get_temperature()

    def calibrate(self, reference: Any) -> None:
        print(f"Calibrating against {reference}°C")

    def get_temperature(self) -> float:
        return self.temperature + np.random.normal(0, 0.1)

class UVVisSpectrometer(Instrument, Spectrometer, Measurable, Calibratable):
    """UV-Vis spectrometer - optical capabilities."""

    def __init__(self):
        self.wavelength = 550.0
        self.initialized = False

    def initialize(self) -> None:
        self.initialized = True

    def shutdown(self) -> None:
        self.initialized = False

    def measure(self) -> float:
        return np.random.rand()

    def calibrate(self, reference: Any) -> None:
        print("Calibrating spectrometer")

    def set_wavelength(self, wavelength: float) -> None:
        self.wavelength = wavelength

    def get_spectrum(self) -> np.ndarray:
        wavelengths = np.linspace(200, 800, 100)
        intensities = np.random.rand(100)
        return np.column_stack([wavelengths, intensities])

class ScanningTunnelMicroscope(Instrument, Scanner, Measurable):
    """STM - scanning and measuring, but not optical."""

    def initialize(self) -> None:
        print("STM initialized")

    def shutdown(self) -> None:
        print("STM shutdown")

    def measure(self) -> float:
        return np.random.rand()

    def start_scan(self) -> None:
        print("Starting scan")

    def stop_scan(self) -> None:
        print("Stopping scan")

```

```python
# Now functions specify exactly what they need!

def read_temperature(sensor: TemperatureSensor) -> float:
    """Read temperature from any temperature sensor."""
    return sensor.get_temperature()

def acquire_spectrum(spec: Spectrometer) -> np.ndarray:
    """Acquire spectrum from any spectrometer."""
    spec.set_wavelength(550.0)
    return spec.get_spectrum()

def calibrate_if_possible(instrument: Instrument, reference: Any) -> None:
    """Calibrate instrument if it supports calibration."""
    if isinstance(instrument, Calibratable):
        instrument.calibrate(reference)
        print("Calibrated")
    else:
        print("Instrument doesn't support calibration")

```

```python
# USAGE: Type-safe and clear!
thermocouple = Thermocouple(1)
temp = read_temperature(thermocouple)  # ✓ Works

uvvis = UVVisSpectrometer()
spectrum = acquire_spectrum(uvvis)  # ✓ Works

# Graceful handling of optional features
calibrate_if_possible(thermocouple, 25.0)  # Calibrate with temperature reference
calibrate_if_possible(uvvis, reference_spectrum)  # Calibrate with spectrum reference

stm = ScanningTunnelMicroscope()
calibrate_if_possible(stm, None)  # Prints "doesn't support calibration"

# Type checker prevents errors!
# spectrum = acquire_spectrum(thermocouple)  # ✗ Type error - not a Spectrometer
# temp = read_temperature(uvvis)  # ✗ Type error - not a TemperatureSensor
```

## Why This Is Better

### 1. Implement Only What You Support

`Thermocouple` implements only the 5 methods it actually supports instead of all 10 interface methods. No dummy implementations for the 5 it doesn't support!

### 2. Type Safety

Functions declare exactly what they need:

```python
def read_temperature(sensor: TemperatureSensor)
```

Type checkers verify you're passing compatible sources at compile time.

### 3. Clear Capabilities

Looking at class declaration tells you exactly what it supports:

```python
class UVVisSpectrometer(Instrument, Spectrometer, Measurable, Calibratable)
```

### 4. Flexible Combinations

Different sources implement different combinations of interfaces based on their actual capabilities.

### 5. Graceful Feature Detection

```python
if isinstance(instrument, Calibratable):
    instrument.calibrate(reference_sample)
```

Code can check for optional features without causing errors.

## Python-Specific: Protocols vs ABCs

Python 3.8+ added Protocols for structural typing (duck typing with type hints):

```python
from typing import Protocol

class TemperatureSensor(Protocol):
    """Temperature sensor as a Protocol - no inheritance needed!"""

    def get_temperature(self) -> float:
        """Read temperature."""
        ...

# Any class with a get_temperature() method is automatically a TemperatureSensor!
class MyCustomThermometer:
    def get_temperature(self) -> float:
        return 25.0 + np.random.normal(0, 0.1)

def monitor(sensor: TemperatureSensor):
    """Works with anything that has get_temperature()."""
    return sensor.get_temperature()

# No inheritance needed!
custom = MyCustomThermometer()
monitor(custom)  # Type checker is happy
```

Use **Protocols** when:

- You want duck typing with type checking
- Third-party classes can't inherit from your interfaces
- You want minimal coupling

Use **ABCs** when:

- You want to enforce implementation
- You want to provide shared method implementations
- You want explicit interface contracts

## Role Interfaces vs Interface Segregation

**Question**: "If ISP says split interfaces, why would I ever combine them?"

**Answer**: Role interfaces compose smaller interfaces when multiple capabilities are **genuinely always needed together** for a specific role:

```python
# ROLE: Environmental monitoring station
class EnvironmentalMonitor(TemperatureSensor, PressureSensor):
    """Must monitor both temperature and pressure."""
    pass

# ROLE: Analytical instrument
class AnalyticalInstrument(Measurable, Calibratable):
    """Research-grade instruments need measurement and calibration."""
    pass

# Specific implementation combines roles
class WeatherStation(EnvironmentalMonitor, AnalyticalInstrument):
    """Weather station is both an environmental monitor and an analytical instrument."""
    pass
```

This is fine! Role interfaces compose smaller interfaces. The key: don't force implementation of methods that aren't needed.

## Testing with Segregated Interfaces

With fat interface, would need to mock everything:

```python
class TestWithFatInterface(unittest.TestCase):
    def test_get_temperature_old_way(self):
        mock_instrument = Mock(spec=Instrument)
        # Must mock 10 methods even though we only use read()!
        mock_instrument.get_temperature.return_value = 25
        mock_instrument.get_pressure = Mock()
        mock_instrument.set_wavelength = Mock()
        mock_instrument.get_spectrum = Mock()
        # ... more mocks we don't even use!

        result = read_temperature(mock_instrument)
```

Small interfaces make testing easier:

```python
import unittest
from unittest.mock import Mock

class TestInstrumentReading(unittest.TestCase):
    def test_measure_temperature(self):
        """Test with minimal mock - only need get_temperature() method."""
        mock_sensor = Mock(spec=TemperatureSensor)
        mock_sensor.get_temperature.return_value = 25.0

        result = read_temperature(mock_sensor)

        self.assertEqual(result, 25.0)
        mock_sensor.get_temperature.assert_called_once()
```

## Real-World Consequences of Interface Bloat

When interfaces force dummy implementations, you create **false promises**:

**Data processing pipeline**:

- Interface promises all instruments support scanning
- Both the `Thermocouple` and `UVVisSpectrometer` implementations throw `NotImplementedError`
- Pipeline code calls `start_scan()` on all sources
- Works fine during testing (test data happens to be scannable)
- Crashes in production when using a thermocouple
- Hours of debugging to find the "not implemented" error

**The problem**: The interface **lied**. It claimed all data sources could be scanned, but they can't. The type system gave false security.

**Better approach**: Segregate interfaces so clients depend only on what they actually need. Scannability becomes an optional feature you check for, not a mandatory method you hope works.

## Red Flags That You Need ISP

Watch for these warning signs:

- Multiple `raise NotImplementedError` in a class
- Many empty method implementations that just `pass`
- Methods that return `None` or empty values as placeholders
- Documentation that says "not all implementations support this"
- `isinstance()` checks before calling interface methods
- Implementations that only use 20-30% of interface methods
- Comments like "TODO: implement this" that never get done
- Clients that only ever call 2-3 methods from a 10-method interface
- Different implementations consistently leave different methods unimplemented

If you're writing more `NotImplementedError` than actual implementation, your interface needs segregation.

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

### Mistake 3: Splitting Based on Implementation, Not Clients

```python
# WRONG: Split by what implementations have
class FileSystemSource(ABC):
    """Everything file-based sources might do."""
    pass

class NetworkSource(ABC):
    """Everything network sources might do."""
    pass

# RIGHT: Split by what clients need
class Readable(ABC):
    """Clients that read data."""
    pass

class Cacheable(ABC):
    """Clients that need caching."""
    pass
```

ISP is about client needs, not implementation details.

## When to Segregate Interfaces

**Segregate when:**

- Different clients need different subsets of functionality
- Implementations often don't support all features
- You find yourself writing many `raise NotImplementedError`
- You have optional features that not all implementations provide

**Don't segregate when:**

- All implementations genuinely need all methods
- The methods are tightly coupled (can't use one without the others)
- You're creating an internal class, not a public interface
- Segregation would create more confusion than clarity

## The Client Perspective

ISP is about clients (code that uses interfaces), not implementations:

```python
# This function only needs temperature capability
def monitor_temperature(sensor: TemperatureSensor):
    temp = sensor.get_temperature()
    return temp

# Works with ANY temperature sensor!
monitor_temperature(thermocouple)
monitor_temperature(weather_station)
monitor_temperature(temp_probe)

# Doesn't care about pressure, wavelength, scanning, etc.
```

## Performance Notes

Segregated interfaces don't add performance overhead. The cost of implementing multiple small interfaces is zero at runtime—it's just a compile-time organization tool.

The real performance benefit: clients can depend on minimal interfaces, loading only necessary dependencies. A function needing only `TemperatureSensor` doesn't load spectrometer libraries, scanning hardware drivers, or pressure calibration modules.

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

**Think of it like lab equipment**: A pH meter doesn't need a "set wavelength" method just because it's an instrument. Design interfaces for what clients need, not for every possible instrument feature.

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

## Your Turn

1. Find a large interface in your code
2. List which implementations support which methods
3. Group methods by which clients use them together
4. Split into smaller interfaces
5. Update implementations to only implement what they support

In the final post of this series, we'll bring together all five SOLID principles and discuss when to apply them (and when not to) in real scientific programming scenarios.

---

*Have you written `raise NotImplementedError` more times than you'd like? Share your interface horror stories in the comments!*

**Previous posts in this series:**

- [Single Responsibility Principle for Scientists](../single_responsibility_principle)
- [Open/Closed Principle for Scientists](../open-closed_principle)
- [Liskov Substitution Principle for Scientists](../liskov_substitution_principle)

**Next in this series:**

- The Dependency Inversion Principle for Scientists - Coming next week
