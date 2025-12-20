+++
title = "Depend on Abstractions: Making Scientific Code Testable and Flexible"
date = 2026-02-02
publishDate = 2026-02-02
draft = true
showAuthor = false
sharingLinks = true
categories = [
    "SOLID Principles",
]
tags = [
    "Dependency Inversion Principle",
]
image = ""
description = ""
toc = true
+++

*Part 5 of the SOLID Principles for Scientific Programmers series*

## The Problem Every Scientist Has Faced

You've written a data acquisition script that reads from your laboratory sensor, processes the data, and saves results to a file. It works great! Then your advisor asks: "Can you run the analysis on last month's data to compare?" Or: "Can you test this on simulated data before we run the experiment?" Or worst of all: "The sensor is broken—can you still work on the analysis code?"

Suddenly you realize your code is completely locked to that one specific sensor and that one specific file format. You can't test anything without the physical hardware connected. You can't reuse the analysis logic with different data sources. Your beautiful working code has become a rigid, untestable monolith.

This is the problem the **Dependency Inversion Principle (DIP)** solves.

## What Is the Dependency Inversion Principle?

The Dependency Inversion Principle states:

> **High-level modules should not depend on low-level modules. Both should depend on abstractions.**
>
> **Abstractions should not depend on details. Details should depend on abstractions.**

In plain English: Your important scientific logic shouldn't be hardwired to specific hardware, file formats, or external services. Instead, it should depend on *interfaces* or *abstract descriptions* of what it needs. The concrete implementations can then be swapped out as needed.

## A Real Scientific Example: The Problem

Let's look at a temperature monitoring system for a materials science experiment. Here's what many scientists write first:

```python
import serial
import csv
from datetime import datetime

class TemperatureMonitor:
    def __init__(self):
        # Hardcoded dependency on specific hardware
        self.sensor = serial.Serial('/dev/ttyUSB0', baudrate=9600)
        # Hardcoded dependency on specific file format
        self.output_file = 'temperature_data.csv'

    def collect_data(self, duration_seconds):
        """Collect temperature data for specified duration."""
        results = []
        start_time = datetime.now()

        while (datetime.now() - start_time).seconds < duration_seconds:
            # Read from serial sensor
            raw_data = self.sensor.readline()
            temperature = float(raw_data.decode().strip())
            timestamp = datetime.now()

            results.append((timestamp, temperature))

            # Check if temperature is in safe range
            if temperature > 100:
                print(f"WARNING: Temperature {temperature}°C exceeds safe limit!")

        # Save to CSV
        with open(self.output_file, 'w', newline='') as f:
            writer = csv.writer(f)
            writer.writerow(['Timestamp', 'Temperature'])
            writer.writerows(results)

        return results

# Usage
monitor = TemperatureMonitor()
data = monitor.collect_data(3600)  # Collect for 1 hour
```

This code works, but it has serious problems:

1. **Can't test without hardware**: You need the physical sensor connected to run any tests
2. **Can't reuse the logic**: The safety check and data collection logic is tied to this specific sensor
3. **Can't work with historical data**: No way to run the same analysis on previously collected data
4. **Can't switch output formats**: What if you want to save to a database instead of CSV?
5. **Can't simulate failures**: How do you test what happens when the sensor malfunctions?

## The Solution: Dependency Inversion

Let's refactor this using the Dependency Inversion Principle. First, we define abstractions for the things we depend on:

```python
from abc import ABC, abstractmethod
from datetime import datetime
from typing import List, Tuple

# ABSTRACTION: What we need from a temperature source
class TemperatureSensor(ABC):
    """Abstract interface for any temperature data source."""

    @abstractmethod
    def read_temperature(self) -> float:
        """Read current temperature in Celsius."""
        pass

# ABSTRACTION: What we need from a data storage mechanism
class DataStorage(ABC):
    """Abstract interface for storing temperature measurements."""

    @abstractmethod
    def save(self, data: List[Tuple[datetime, float]]) -> None:
        """Save temperature data."""
        pass

# HIGH-LEVEL MODULE: Now depends only on abstractions
class TemperatureMonitor:
    def __init__(self, sensor: TemperatureSensor, storage: DataStorage):
        # Dependencies are injected, not created internally
        self.sensor = sensor
        self.storage = storage

    def collect_data(self, duration_seconds: int) -> List[Tuple[datetime, float]]:
        """Collect temperature data for specified duration."""
        results = []
        start_time = datetime.now()

        while (datetime.now() - start_time).seconds < duration_seconds:
            temperature = self.sensor.read_temperature()
            timestamp = datetime.now()

            results.append((timestamp, temperature))

            # Business logic is now independent of hardware details
            if temperature > 100:
                print(f"WARNING: Temperature {temperature}°C exceeds safe limit!")

        self.storage.save(results)
        return results
```

Now we create concrete implementations of our abstractions:

```python
import serial
import csv

# CONCRETE IMPLEMENTATION: Real serial sensor
class SerialTemperatureSensor(TemperatureSensor):
    def __init__(self, port: str, baudrate: int = 9600):
        self.sensor = serial.Serial(port, baudrate=baudrate)

    def read_temperature(self) -> float:
        raw_data = self.sensor.readline()
        return float(raw_data.decode().strip())

# CONCRETE IMPLEMENTATION: CSV storage
class CSVStorage(DataStorage):
    def __init__(self, filename: str):
        self.filename = filename

    def save(self, data: List[Tuple[datetime, float]]) -> None:
        with open(self.filename, 'w', newline='') as f:
            writer = csv.writer(f)
            writer.writerow(['Timestamp', 'Temperature'])
            writer.writerows(data)

# Production usage - same as before
monitor = TemperatureMonitor(
    sensor=SerialTemperatureSensor('/dev/ttyUSB0'),
    storage=CSVStorage('temperature_data.csv')
)
data = monitor.collect_data(3600)
```

## Why This Is Better: Flexibility Unlocked

Now that we've inverted the dependencies, we can easily create alternative implementations:

### 1. Testing Without Hardware

```python
import random

class MockTemperatureSensor(TemperatureSensor):
    """Simulated sensor for testing."""

    def __init__(self, base_temp: float = 25.0, noise: float = 0.5):
        self.base_temp = base_temp
        self.noise = noise

    def read_temperature(self) -> float:
        # Simulate realistic temperature readings
        return self.base_temp + random.uniform(-self.noise, self.noise)

class InMemoryStorage(DataStorage):
    """Store data in memory for testing."""

    def __init__(self):
        self.data = []

    def save(self, data: List[Tuple[datetime, float]]) -> None:
        self.data = data

# Now we can test without any hardware!
test_monitor = TemperatureMonitor(
    sensor=MockTemperatureSensor(base_temp=25.0),
    storage=InMemoryStorage()
)
test_data = test_monitor.collect_data(60)
print(f"Collected {len(test_data)} test measurements")
```

### 2. Testing Edge Cases

```python
class FailingTemperatureSensor(TemperatureSensor):
    """Simulate sensor failures for testing error handling."""

    def __init__(self, fail_after: int = 10):
        self.read_count = 0
        self.fail_after = fail_after

    def read_temperature(self) -> float:
        self.read_count += 1
        if self.read_count > self.fail_after:
            raise IOError("Sensor connection lost!")
        return 25.0

# Test failure handling
monitor = TemperatureMonitor(
    sensor=FailingTemperatureSensor(fail_after=5),
    storage=InMemoryStorage()
)
# This will raise an exception - now you can test your error handling!
```

### 3. Working With Historical Data

```python
class HistoricalDataSensor(TemperatureSensor):
    """Replay previously recorded data."""

    def __init__(self, filename: str):
        with open(filename, 'r') as f:
            reader = csv.reader(f)
            next(reader)  # Skip header
            self.temperatures = [float(row[1]) for row in reader]
        self.index = 0

    def read_temperature(self) -> float:
        if self.index >= len(self.temperatures):
            raise StopIteration("No more historical data")
        temp = self.temperatures[self.index]
        self.index += 1
        return temp

# Analyze last week's data with the same code!
historical_monitor = TemperatureMonitor(
    sensor=HistoricalDataSensor('last_week_data.csv'),
    storage=InMemoryStorage()
)
```

### 4. Different Output Formats

```python
import json
import sqlite3

class JSONStorage(DataStorage):
    """Save data as JSON."""

    def __init__(self, filename: str):
        self.filename = filename

    def save(self, data: List[Tuple[datetime, float]]) -> None:
        json_data = [
            {"timestamp": ts.isoformat(), "temperature": temp}
            for ts, temp in data
        ]
        with open(self.filename, 'w') as f:
            json.dump(json_data, f, indent=2)

class DatabaseStorage(DataStorage):
    """Save data to SQLite database."""

    def __init__(self, db_path: str):
        self.conn = sqlite3.connect(db_path)
        self.conn.execute('''
            CREATE TABLE IF NOT EXISTS temperatures
            (timestamp TEXT, temperature REAL)
        ''')

    def save(self, data: List[Tuple[datetime, float]]) -> None:
        self.conn.executemany(
            'INSERT INTO temperatures VALUES (?, ?)',
            [(ts.isoformat(), temp) for ts, temp in data]
        )
        self.conn.commit()

# Same monitoring code, different storage!
monitor_json = TemperatureMonitor(
    sensor=SerialTemperatureSensor('/dev/ttyUSB0'),
    storage=JSONStorage('temps.json')
)

monitor_db = TemperatureMonitor(
    sensor=SerialTemperatureSensor('/dev/ttyUSB0'),
    storage=DatabaseStorage('temps.db')
)
```

## Scientists' Gotcha: Over-Abstraction

A common mistake is to abstract *everything*. You don't need interfaces for simple mathematical functions or well-established libraries like NumPy. Apply DIP when:

✅ **DO use DIP for:**

- Hardware interfaces (sensors, instruments, actuators)
- External data sources (files, databases, APIs)
- Output mechanisms (file formats, plotting, reporting)
- Complex algorithms you might want to swap (numerical solvers, fitting methods)

❌ **DON'T use DIP for:**

- Basic math operations (`np.mean()`, `np.std()`)
- Standard library functions
- Simple utility functions
- Things that will never change

## The Testing Advantage

The real power of DIP becomes clear when writing tests. Here's a complete example:

```python
import unittest

class TestTemperatureMonitor(unittest.TestCase):
    def setUp(self):
        """Set up test fixtures."""
        self.storage = InMemoryStorage()
        self.sensor = MockTemperatureSensor(base_temp=25.0)
        self.monitor = TemperatureMonitor(self.sensor, self.storage)

    def test_collects_data(self):
        """Test that data collection works."""
        data = self.monitor.collect_data(5)
        self.assertGreater(len(data), 0)
        self.assertEqual(len(self.storage.data), len(data))

    def test_temperature_in_range(self):
        """Test that temperatures are reasonable."""
        data = self.monitor.collect_data(5)
        for timestamp, temp in data:
            self.assertGreater(temp, 20)
            self.assertLess(temp, 30)

    def test_handles_high_temperature(self):
        """Test warning for high temperatures."""
        hot_sensor = MockTemperatureSensor(base_temp=150.0)
        monitor = TemperatureMonitor(hot_sensor, self.storage)

        # Would print warnings, but doesn't crash
        data = monitor.collect_data(5)
        self.assertGreater(len(data), 0)

# Run tests without any hardware connected!
if __name__ == '__main__':
    unittest.main()
```

No hardware, no external files, no network—just fast, reliable tests.

## Real-World Benefits

After applying DIP to your code, you can:

1. **Develop on your laptop** while the lab equipment is in use
2. **Test edge cases** that are dangerous or impossible to create with real hardware
3. **Run automated tests** in your CI/CD pipeline
4. **Work with simulated data** while waiting for experiments to complete
5. **Easily switch** between development and production configurations
6. **Reuse your analysis logic** with completely different data sources

## Practical Refactoring Strategy

If you have existing code that's tightly coupled, here's how to refactor it:

**Step 1**: Identify your dependencies (hardware, file I/O, external services)

**Step 2**: Create abstract interfaces for each dependency

**Step 3**: Refactor your main class to accept dependencies through its constructor

**Step 4**: Create concrete implementations of the abstractions

**Step 5**: Create mock/test implementations

**Step 6**: Update your code to inject dependencies

You don't have to do this all at once! Start with the dependency that causes you the most pain (usually hardware).

## Language Considerations

### Python (Duck Typing)

Python doesn't require explicit interfaces, but using `ABC` (Abstract Base Classes) makes your intent clear and catches mistakes:

```python
from abc import ABC, abstractmethod

class Sensor(ABC):
    @abstractmethod
    def read(self): pass
```

### Statically Typed Languages

In languages like C++, Java, or Julia with type systems, DIP is even more powerful because the compiler enforces the abstractions:

```julia
# Julia example
abstract type TemperatureSensor end

function read_temperature(sensor::TemperatureSensor)
    error("Must implement read_temperature for $(typeof(sensor))")
end

struct SerialSensor <: TemperatureSensor
    port::String
end

read_temperature(s::SerialSensor) = # implementation
```

## When to Skip DIP

DIP adds upfront complexity. Skip it for:

- **Quick exploratory scripts** that you'll run once and throw away
- **One-person projects** where you're certain the dependencies won't change
- **Very simple programs** with no testing requirements
- **Prototypes** where you're still figuring out what you need

But consider adding it later when:

- The code becomes mission-critical
- Multiple people need to work on it
- You need automated testing
- The dependencies start causing pain

## Summary

The Dependency Inversion Principle transforms rigid, hardware-dependent scientific code into flexible, testable software. By depending on abstractions instead of concrete implementations, you can:

- Test without hardware
- Reuse analysis logic with different data sources
- Swap components easily
- Simulate edge cases
- Work on code while equipment is unavailable

The key insight: Your valuable scientific logic should be independent of the messy details of how data gets in and out of your system.

## Next Steps

1. **Identify one pain point** in your current code where DIP would help
2. **Create an abstraction** for that dependency
3. **Implement a mock version** for testing
4. **Refactor gradually**—you don't have to do everything at once

In the next post, we'll bring together all five SOLID principles and discuss when to apply them (and when not to) in real scientific programming scenarios.

---

*Have questions or examples from your own scientific code? Share them in the comments below!*

**Previous posts in this series:**

- [Single Responsibility Principle for Scientists](srp_blog_post.md)
- [Open/Closed Principle for Scientists](ocp_blog_post.md)
- [Liskov Substitution Principle for Scientists](lsp_blog_post.md)
- [Interface Segregation Principle for Scientists](isp_blog_post.md)

**Coming next:**

- [Putting It All Together: SOLID in Practice for Scientific Code](solid_practice_blog.md)
