+++
title = "When Your Cheap Sensor Breaks Everything: Understanding LSP"
date = "2026-01-18"
publishDate = "2026-01-19"
draft = false
showAuthor = true
sharingLinks = true
image = "/images/solid/liskov_hero.png"
description = "Your cheap sensor returns Fahrenheit instead of Celsius, breaking all your results. Learn the Liskov Substitution Principle to prevent subclass surprises."
categories = [
    "SOLID Principles",
]
tags = [
    "LSP"
]
series = ["SOLID Principles"]
series_order = 4
toc = true
+++

*Part 3 of the SOLID Principles for Scientific Programmers series*

## The Sensor Swap Disaster

You've built a data acquisition system that works perfectly with your lab's standard temperature sensor. The code collects data, applies calibration corrections, and flags readings outside the valid range. Everything is tested and working.

Then you need to swap in a different sensor model—maybe the original is broken, or you're using equipment at a collaborator's lab. The new sensor is also a temperature sensor, so you create a `CheapTemperatureSensor` subclass. The code runs without errors.

But then you notice something wrong: your analysis is giving bizarre results. After hours of debugging, you discover the cheap sensor returns values in Fahrenheit instead of Celsius.  Your results are off by a factor of ~2, and other colleagues have based their experiments on your flawed data.

You used inheritance, but you broke a critical principle: **substitutability**.

This is what the **Liskov Substitution Principle (LSP)** prevents.

## What Is the Liskov Substitution Principle?

Barbara Liskov stated it formally as:

> **If S is a subtype of T, then objects of type T may be replaced with objects of type S without altering any of the desirable properties of the program.**

In simpler terms for scientists: **If you inherit from a class, the child class should be a drop-in replacement for the parent. Code using the parent class should work correctly with any child class without knowing the difference.**

Think of it like equipment standards in a lab: If your experiment needs a pressure sensor with ±1% accuracy reporting in kPa, any sensor meeting that spec should work. You shouldn't need to rewrite your experiment for each sensor brand—as long as they honour the spec. If a sensor reports in PSI instead, or only works in a narrower range, it violates the "standard"—and your experiment breaks.

## Before You Refactor: Is It Worth It?

LSP violations often hide until you try to substitute classes. Before refactoring, consider:

- **Do you have multiple implementations of the same concept?** (multiple sensors, readers, algorithms)
- **Does code break when you swap implementations?** This is the clearest LSP violation signal
- **Are you using inheritance?** If no inheritance, LSP doesn't apply
- **Do different implementations have different behavior contracts?** (units, ranges, capabilities)

If you have multiple implementations that should be interchangeable but aren't, LSP refactoring is essential.

## A Real Example: The Problem

Let's look at a sensor abstraction that *seems* reasonable but violates LSP:

```python
import numpy as np
from datetime import datetime
from dataclasses import dataclass
from typing import Optional

@dataclass
class Measurement:
    """A measurement with value and uncertainty."""
    value: float
    uncertainty: float
    units: str
    timestamp: datetime

class TemperatureSensor:
    """Base class for temperature sensors."""

    def __init__(self, sensor_id: str):
        self.sensor_id = sensor_id
        self.calibration_offset = 0.0

    def read(self) -> Measurement:
        """Read temperature in Celsius with uncertainty."""
        # Placeholder - subclasses will implement
        raise NotImplementedError

    def calibrate(self, reference_temp: float, measured_temp: float):
        """Calibrate sensor against known reference."""
        self.calibration_offset = reference_temp - measured_temp
        print(f"Sensor {self.sensor_id} calibrated: offset = {self.calibration_offset:.2f}°C")

    def is_reading_valid(self, measurement: Measurement) -> bool:
        """Check if reading is within valid sensor range."""
        # Most lab sensors work in range -50 to 200°C
        return -50 <= measurement.value <= 200

class PrecisionTemperatureSensor(TemperatureSensor):
    """High-precision lab sensor."""

    def read(self) -> Measurement:
        # Simulate reading from hardware
        raw_value = 25.0 + np.random.normal(0, 0.1)
        corrected_value = raw_value + self.calibration_offset

        return Measurement(
            value=corrected_value,
            uncertainty=0.1,
            units="Celsius",
            timestamp=datetime.now()
        )

class CheapTemperatureSensor(TemperatureSensor):
    """Low-cost sensor with limitations."""

    def read(self) -> Measurement:
        # Simulate reading from cheap hardware
        raw_value = 77.0 + np.random.normal(0, 2.0)  # Returns Fahrenheit!

        # Doesn't apply calibration offset (hardware limitation)
        # Doesn't provide uncertainty (not capable)

        return Measurement(
            value=raw_value,  # Wrong units!
            uncertainty=0.0,   # No uncertainty info!
            units="Fahrenheit",  # Different units!
            timestamp=datetime.now()
        )

    def calibrate(self, reference_temp: float, measured_temp: float):
        """This sensor can't be calibrated."""
        print(f"Warning: {self.sensor_id} does not support calibration")
        # Doesn't set calibration_offset!

class FastTemperatureSensor(TemperatureSensor):
    """High-speed sensor with reduced range."""

    def read(self) -> Measurement:
        raw_value = 25.0 + np.random.normal(0, 0.5)
        corrected_value = raw_value + self.calibration_offset

        return Measurement(
            value=corrected_value,
            uncertainty=0.5,
            units="Celsius",
            timestamp=datetime.now()
        )

    def is_reading_valid(self, measurement: Measurement) -> bool:
        """Fast sensor only works in narrow range."""
        return 0 <= measurement.value <= 100  # Narrower range!

# Code that uses sensors
class ExperimentRunner:
    """Runs an experiment with temperature monitoring."""

    def __init__(self, sensor: TemperatureSensor):
        self.sensor = sensor
        self.data = []

    def run_experiment(self, duration_seconds: int):
        """Collect data for specified duration."""
        print(f"Starting experiment with {self.sensor.sensor_id}")

        for i in range(duration_seconds):
            measurement = self.sensor.read()

            # Assume all measurements are in Celsius!
            if not self.sensor.is_reading_valid(measurement):
                print(f"WARNING: Invalid reading: {measurement.value}°C")
                continue

            # Assume uncertainty is always provided
            if measurement.uncertainty > 1.0:
                print(f"WARNING: High uncertainty: {measurement.uncertainty}°C")

            self.data.append(measurement)

        self._analyze_results()

    def _analyze_results(self):
        """Analyze collected data."""
        values = [m.value for m in self.data]
        uncertainties = [m.uncertainty for m in self.data]

        mean_temp = np.mean(values)
        mean_uncertainty = np.mean(uncertainties)

        print(f"\nResults:")
        print(f"  Mean temperature: {mean_temp:.2f}°C")
        print(f"  Mean uncertainty: {mean_uncertainty:.2f}°C")
        print(f"  Samples collected: {len(self.data)}")

# Usage - looks fine at first!
print("=== Precision Sensor ===")
precise_sensor = PrecisionTemperatureSensor("TEMP-001")
precise_sensor.calibrate(25.0, 25.5)
experiment1 = ExperimentRunner(precise_sensor)
experiment1.run_experiment(10)

print("\n=== Cheap Sensor ===")
cheap_sensor = CheapTemperatureSensor("CHEAP-001")
cheap_sensor.calibrate(25.0, 77.0)  # Thinks it's calibrating!
experiment2 = ExperimentRunner(cheap_sensor)
experiment2.run_experiment(10)  # Results will be nonsense!

print("\n=== Fast Sensor at High Temp ===")
fast_sensor = FastTemperatureSensor("FAST-001")
experiment3 = ExperimentRunner(fast_sensor)
# If temp goes above 100°C, validation fails unexpectedly!
```

## Problems with This Design

This code violates LSP in multiple ways:

### Violation 1: Changed Units

`CheapTemperatureSensor.read()` returns Fahrenheit, but the base class contract expects Celsius. The `ExperimentRunner` has no idea and treats all values as Celsius, producing nonsense results.

### Violation 2: Broken Calibration

`CheapTemperatureSensor.calibrate()` doesn't actually calibrate. Code calling `calibrate()` expects it to work, but it silently fails.

### Violation 3: Missing Uncertainty

`CheapTemperatureSensor` returns `uncertainty=0.0`. Code checking if `uncertainty > 1.0` never triggers, missing important warnings.

### Violation 4: Stricter Validation

`FastTemperatureSensor.is_reading_valid()` has a narrower range than the base class promises. Valid readings for `TemperatureSensor` (150°C) are invalid for `FastTemperatureSensor`.

**The core problem**: You can't safely substitute these subclasses for the base class. Each one breaks expectations in different ways.

## The Solution: Liskov Substitution Principle

Let's redesign this following LSP. The key: **Make sure all subclasses honour the base class contract.**

```ascii
BEFORE (violates LSP):          AFTER (follows LSP):
┌──────────────────┐            ┌──────────────────┐
│ TemperatureSensor│            │ TemperatureSensor│
│ read() → °C      │            │ read() → °C      │
│ calibrate()      │            │ (contract)       │
└──────────────────┘            └──────────────────┘
         △                                △
         │                                 │
    ┌────┴────┐                       ┌────┴────┐
    │         │                       │         │
┌───────┐ ┌────────┐            ┌────────┐ ┌────────┐
│Cheap  │ │Fast    │            │Economy │ │Fast    │
│→ °F ❌│ │0-100❌│            │→ °C ✅ │ │0-100✅│
│no cal❌│ │      │            │cal ✅   │ │       │
└───────┘ └────────┘            └────────┘ └────────┘
   Breaks contract              Honours contract
```

```python
import numpy as np
from datetime import datetime
from dataclasses import dataclass
from typing import Optional
from abc import ABC, abstractmethod

# DATA STRUCTURE: Measurement with validation
@dataclass
class Measurement:
    """A measurement with value and uncertainty in standard units."""
    value: float  # Always in Celsius for temperature
    uncertainty: float  # Always positive, in same units as value
    timestamp: datetime

    # Enforces invariant: uncertainty must be non-negative
    # This prevents subclasses from violating the contract
    def __post_init__(self):
        """Validate measurement invariants."""
        if self.uncertainty < 0:
            raise ValueError("Uncertainty cannot be negative")
```

```python
# BASE CLASS: Defines the contract all sensors must follow
class TemperatureSensor(ABC):
    """
    Base class for temperature sensors.

    CONTRACT:
    - read() returns temperature in Celsius with uncertainty
    - Uncertainty is always >= 0
    - Valid temperature range: -50 to 200°C (expandable via properties)
    - calibrate() adjusts future readings by the given offset
    - All subclasses must honour these guarantees
    """

    def __init__(self, sensor_id: str, valid_range: tuple = (-50, 200)):
        self.sensor_id = sensor_id
        self.calibration_offset = 0.0
        self.min_temp, self.max_temp = valid_range

    @abstractmethod
    def _read_raw(self) -> tuple[float, float]:
        """
        Read raw value and uncertainty from hardware.

        Returns:
            (value_celsius, uncertainty_celsius): Both in Celsius

        Must be implemented by subclasses to handle their specific hardware.

        WHY THIS DESIGN:
        By keeping read() in the base class and making _read_raw() abstract,
        we guarantee that calibration is always applied consistently. If we
        made read() abstract, subclasses might forget to apply calibration_offset,
        violating the contract. This pattern enforces the contract automatically.

        Note: Python doesn't have 'final' keywords to prevent overriding read(),
        so this is a convention. Trust your team to follow it.
        """
        pass

    def read(self) -> Measurement:
        """
        Read temperature in Celsius with uncertainty.

        This method is FINAL - subclasses should not override it.
        Instead, implement _read_raw().
        """
        raw_value, uncertainty = self._read_raw()
        corrected_value = raw_value + self.calibration_offset

        return Measurement(
            value=corrected_value,
            uncertainty=uncertainty,
            timestamp=datetime.now()
        )

    def calibrate(self, reference_temp: float, measured_temp: float):
        """
        Calibrate sensor against known reference.

        All future readings will be adjusted by the calculated offset.
        Subclasses should not override this unless they have special
        calibration procedures.
        """
        self.calibration_offset = reference_temp - measured_temp
        print(f"Sensor {self.sensor_id} calibrated: offset = {self.calibration_offset:.2f}°C")

    def is_reading_valid(self, measurement: Measurement) -> bool:
        """
        Check if reading is within this sensor's valid range.

        Subclasses can have narrower ranges but not wider.
        """
        return self.min_temp <= measurement.value <= self.max_temp

    def get_valid_range(self) -> tuple[float, float]:
        """Return the valid temperature range for this sensor."""
        return (self.min_temp, self.max_temp)
```

```python
# CONCRETE IMPLEMENTATIONS: Each honors the contract

class PrecisionTemperatureSensor(TemperatureSensor):
    """
    High-precision lab sensor.

    - Uncertainty: ±0.1°C
    - Range: -50 to 200°C
    - Supports calibration
    """

    def _read_raw(self) -> tuple[float, float]:
        """Read from precision hardware."""
        # Simulate high-precision hardware (returns Celsius)
        value = 25.0 + np.random.normal(0, 0.1)
        uncertainty = 0.1
        return value, uncertainty

class EconomyTemperatureSensor(TemperatureSensor):
    """
    Low-cost sensor that properly handles its limitations.

    - Higher uncertainty: ±2.0°C
    - Range: -50 to 200°C
    - Hardware returns Fahrenheit but we convert it
    - Supports calibration
    """

    def _read_raw(self) -> tuple[float, float]:
        """Read from economy hardware and convert to Celsius."""
        # Hardware returns Fahrenheit
        value_f = 77.0 + np.random.normal(0, 2.0)

        # Convert to Celsius to honour the contract
        value_c = (value_f - 32) * 5/9

        # Higher uncertainty, also converted
        uncertainty_c = 2.0 * 5/9

        return value_c, uncertainty_c

class FastTemperatureSensor(TemperatureSensor):
    """
    High-speed sensor with reduced range.

    - Uncertainty: ±0.5°C
    - Range: 0 to 100°C (NARROWER than base class)
    - Supports calibration
    - Clearly documents range limitation
    """

    def __init__(self, sensor_id: str):
        # Initialize with narrower range (allowed - not wider!)
        super().__init__(sensor_id, valid_range=(0, 100))

    def _read_raw(self) -> tuple[float, float]:
        """Read from fast hardware."""
        value = 25.0 + np.random.normal(0, 0.5)
        uncertainty = 0.5
        return value, uncertainty

class UncalibratedSensor(TemperatureSensor):
    """
    Sensor that cannot be calibrated (e.g., sealed unit).

    If a sensor truly cannot be calibrated, we make this explicit
    and handle it properly rather than silently failing.
    """

    def _read_raw(self) -> tuple[float, float]:
        """Read from uncalibrated hardware."""
        value = 25.0 + np.random.normal(0, 1.0)
        uncertainty = 1.0
        return value, uncertainty

    def calibrate(self, reference_temp: float, measured_temp: float):
        """
        Uncalibrated sensors raise an error rather than silently failing.
        """
        raise NotImplementedError(
            f"Sensor {self.sensor_id} is a sealed unit and cannot be calibrated. "
            f"Consider using a different sensor model if calibration is required."
        )
```

```python
# Experiment runner works correctly with ALL sensors now
class ExperimentRunner:
    """Runs an experiment with temperature monitoring."""

    def __init__(self, sensor: TemperatureSensor):
        self.sensor = sensor
        self.data = []

        # Check sensor's valid range
        min_t, max_t = sensor.get_valid_range()
        print(f"Sensor {sensor.sensor_id} valid range: {min_t}°C to {max_t}°C")

    def run_experiment(self, duration_seconds: int):
        """Collect data for specified duration."""
        print(f"Starting experiment with {self.sensor.sensor_id}")

        for i in range(duration_seconds):
            measurement = self.sensor.read()

            # Can trust all measurements are in Celsius!
            if not self.sensor.is_reading_valid(measurement):
                print(f"WARNING: Reading {measurement.value:.1f}°C outside valid range")
                continue

            # Can trust uncertainty is always present and meaningful
            if measurement.uncertainty > 1.0:
                print(f"WARNING: High uncertainty: {measurement.uncertainty:.2f}°C")

            self.data.append(measurement)

        self._analyze_results()

    def _analyze_results(self):
        """Analyze collected data."""
        if not self.data:
            print("No valid data collected!")
            return

        values = [m.value for m in self.data]
        uncertainties = [m.uncertainty for m in self.data]

        mean_temp = np.mean(values)
        mean_uncertainty = np.mean(uncertainties)

        print(f"\nResults:")
        print(f"  Mean temperature: {mean_temp:.2f}°C")
        print(f"  Mean uncertainty: {mean_uncertainty:.2f}°C")
        print(f"  Samples collected: {len(self.data)}")
```

```python
# Usage - everything works correctly now!
print("=== Precision Sensor ===")
precise = PrecisionTemperatureSensor("PREC-001")
precise.calibrate(25.0, 25.5)
exp1 = ExperimentRunner(precise)
exp1.run_experiment(10)

print("\n=== Economy Sensor ===")
economy = EconomyTemperatureSensor("ECON-001")
economy.calibrate(25.0, 77.0)  # Works correctly despite F→C conversion
exp2 = ExperimentRunner(economy)
exp2.run_experiment(10)  # Results are correct!

print("\n=== Fast Sensor ===")
fast = FastTemperatureSensor("FAST-001")
exp3 = ExperimentRunner(fast)  # Range clearly documented
exp3.run_experiment(10)

print("\n=== Uncalibrated Sensor ===")
uncalibrated = UncalibratedSensor("UNCAL-001")
try:
    uncalibrated.calibrate(25.0, 26.0)  # Fails explicitly
except NotImplementedError as e:
    print(f"Expected error: {e}")
exp4 = ExperimentRunner(uncalibrated)
exp4.run_experiment(10)  # But reading still works!
```

## Why This Is Better

### 1. Consistent Return Types and Units

All sensors return `Measurement` objects with temperature in Celsius. The economy sensor does its conversion internally.

### 2. Honour Preconditions and Postconditions

- **Precondition**: Sensors can be read at any time
- **Postcondition**: `read()` always returns valid `Measurement` with non-negative uncertainty
- All subclasses honour this contract

### 3. Explicit About Limitations

`UncalibratedSensor` raises an error for calibration rather than silently failing. This seems like it violates LSP (throwing an exception the base doesn't), but there's a key distinction:

- Violating LSP: Throwing exceptions for VALID inputs (e.g., raising error when calibration values are in expected range)
- Honouring LSP: Throwing exceptions that indicate a capability limitation (`NotImplementedError` says "this operation isn't supported")

Better alternatives exist (like checking a `can_calibrate()` flag), but raising `NotImplementedError` is the pragmatic Python convention for "this subclass doesn't support this operation."

### 4. Range Restrictions Are Clear

The `FastTemperatureSensor` demonstrates an LSP-compliant way to have tighter restrictions. The base class contract allows subclasses to be MORE restrictive (narrower range, higher precision), but not LESS restrictive (wider range, lower precision).

However, this creates a practical problem: if your experiment needs to measure 150°C, the fast sensor will reject valid readings. This is LSP-compliant but might not be what you want.

**The trade-off**:

- ✅ Code doesn't silently fail (you get explicit warnings about invalid range)
- ✅ The contract is honored (fast sensor doesn't lie about capabilities)
- ❌ You still need to know which sensor is appropriate for your experiment

This is why documenting sensor capabilities and checking ranges upfront (as the `ExperimentRunner` class does) is critical. LSP prevents silent failures, but it doesn't eliminate the need to choose the right tool for the job.

### 5. No Surprises

Any code written for `TemperatureSensor` works correctly with any subclass.

## Design by Contract

LSP is really about **contracts**. The base class establishes a contract:

```python
class TemperatureSensor:
    """
    CONTRACT:

    PRECONDITIONS (what caller must ensure):
    - Sensor is properly initialized
    - For calibrate(): reference and measured temps are valid numbers

    POSTCONDITIONS (what class guarantees):
    - read() returns Measurement in Celsius
    - Uncertainty is non-negative
    - Valid range is defined
    - calibrate() adjusts future readings (or raises NotImplementedError)

    INVARIANTS (always true):
    - calibration_offset is a number
    - valid_range is a tuple of (min, max) where min < max
    """
```

Subclasses can:

- ✅ Accept weaker preconditions (be more lenient about inputs)
- ✅ Provide stronger postconditions (give more guarantees)
- ✅ Maintain all invariants

Subclasses cannot:

- ❌ Require stronger preconditions (be more demanding)
- ❌ Provide weaker postconditions (give fewer guarantees)
- ❌ Break invariants

For example:

```python
# VIOLATES CONTRACT: Weaker postcondition
class UnreliableSensor(TemperatureSensor):
    def read(self) -> Measurement:
        # Sometimes returns None! Base class promises Measurement
        if self.hardware_broken:
            return None  # ❌ Breaks contract

# HONOURS CONTRACT: Stronger postcondition
class ReliableSensor(TemperatureSensor):
    def read(self) -> Measurement:
        # Always returns valid Measurement, sometimes with retry
        for attempt in range(3):
            result = self._try_read()
            if result.uncertainty < 2.0:
                return result  # ✅ Always returns valid Measurement
        return self._get_safe_default()
```

## Real-World Consequences of LSP Violations

When you violate LSP, you create **silent failures** that are expensive to debug:

**Timeline of a typical LSP violation disaster:**

- **Day 1**: Student A writes experiment code with `PrecisionSensor`, tests thoroughly, everything works
- **Day 30**: Student B needs to run same experiment but `PrecisionSensor` is in use
- **Day 30**: Student B swaps in `CheapSensor` (it's a subclass, should work!)
- **Day 31-60**: Student B collects 30 days of data, code runs without errors
- **Day 61**: Student B analyzes results, gets bizarre temperature distributions
- **Day 62**: Student B spends hours checking analysis code (which is correct)
- **Day 63**: Student B re-calibrates cheap sensor (doesn't help)
- **Day 64**: Student B checks raw data files, discovers °F in the logs
- **Day 65**: Student B realizes all 30 days of data is corrupted
- **Day 65**: Student B restarts experiment, loses a month of work

**The cost**: 30+ days of wasted data collection, maybe a delayed graduation, or wasted grant funding.

**The problem**: The code didn't **fail**, it **lied**. If `CheapSensor` couldn't pretend to be a `TemperatureSensor`, the error would have been caught on Day 30.

**Better approach**: Honour the contract (convert F→C internally) or don't inherit (use a different type entirely, forcing explicit handling).

## Common LSP Violations in Scientific Code

Here are two patterns that frequently violate LSP in scientific codebases, along with how to fix them:

### Violation 1: Changing Precision or Format

```python
class DataPoint:
    def get_timestamp(self) -> float:
        """Return Unix timestamp in seconds."""
        return self._timestamp

# VIOLATION: Different precision
class HighResolutionDataPoint(DataPoint):
    def get_timestamp(self) -> float:
        """Returns microsecond precision timestamp."""
        return self._timestamp_us  # ❌ Different scale! Off by 10^6
```

### Violation 2: Different Thread-Safety Guarantees

```python
class DataBuffer:
    def append(self, value: float):
        """Add value to buffer. Thread-safe."""
        with self._lock:
            self._data.append(value)

# VIOLATION: Removes thread-safety
class FastDataBuffer(DataBuffer):
    def append(self, value: float):
        """Add value to buffer."""
        self._data.append(value)  # ❌ No lock! Breaks in multithreaded code
```

## Testing for LSP Compliance

Write tests that work for the base class and run them on all subclasses:

```python
import unittest

class TestTemperatureSensor(unittest.TestCase):
    """Tests that should pass for ANY TemperatureSensor subclass."""

    def get_sensor(self) -> TemperatureSensor:
        """Override in subclasses to test specific sensor."""
        return PrecisionTemperatureSensor("TEST-001")

    def test_read_returns_celsius(self):
        """All sensors must return Celsius."""
        sensor = self.get_sensor()
        measurement = sensor.read()
        # Temperature in reasonable range for Celsius
        self.assertGreater(measurement.value, -100)
        self.assertLess(measurement.value, 300)

    def test_uncertainty_is_positive(self):
        """All sensors must provide non-negative uncertainty."""
        sensor = self.get_sensor()
        measurement = sensor.read()
        self.assertGreaterEqual(measurement.uncertainty, 0)

    def test_calibration_affects_readings(self):
        """Calibration should adjust readings (or raise NotImplementedError)."""
        sensor = self.get_sensor()

        try:
            before = sensor.read().value
            sensor.calibrate(before + 1.0, before)
            after = sensor.read().value
            # Should be approximately 1°C higher
            self.assertGreater(after, before + 0.5)
        except NotImplementedError:
            # Acceptable if sensor can't calibrate
            pass

# Test each subclass with the same tests
class TestPrecisionSensor(TestTemperatureSensor):
    def get_sensor(self):
        return PrecisionTemperatureSensor("PREC-TEST")

class TestEconomySensor(TestTemperatureSensor):
    def get_sensor(self):
        return EconomyTemperatureSensor("ECON-TEST")

class TestFastSensor(TestTemperatureSensor):
    def get_sensor(self):
        return FastTemperatureSensor("FAST-TEST")

# All tests should pass for all sensors!
```

```python
class TestCheapSensor(TestTemperatureSensor):
    """This would FAIL if we used the bad CheapSensor implementation."""
    def get_sensor(self):
        return CheapTemperatureSensor("CHEAP-TEST")  # From "before" version

    # test_read_returns_celsius would fail!
    # measurement.value would be ~77 (Fahrenheit) instead of ~25 (Celsius)
```

**Why this works**: If all subclasses pass the same base class tests, they're substitutable. Any code written against `TemperatureSensor` will work with any subclass.

This is the **practical test** for LSP: can you swap implementations without changing calling code?

## Red Flags That You're Violating LSP

Watch for these warning signs:

- Subclass changes return types or data structures
- Subclass returns different units than parent (Celsius vs Fahrenheit)
- Subclass throws exceptions the parent doesn't
- Subclass silently ignores method calls that should work
- You need to check the specific type before calling methods
- Documentation says "works like X except..."
- Tests for base class fail on subclass
- You have `isinstance()` checks in calling code
- Subclass requires additional setup the parent doesn't

If you're checking `isinstance(sensor, CheapTemperatureSensor)` to handle it specially, you've violated LSP.

## Common Mistakes: When Inheritance Isn't the Answer

The biggest LSP mistake: using inheritance when you should use composition.

**Use inheritance for "is-a" relationships:**

- A PrecisionTemperatureSensor **is a** TemperatureSensor
- A QuadraticFit **is a** FitStrategy
- Both implement the same contract completely

**Use composition for "has-a" relationships:**

- A DataAcquisitionSystem **has a** sensor (not "is a" sensor)
- An Experiment **has a** data processor (not "is a" processor)

```python
# Instead of inheriting, compose!
class DataAcquisitionSystem:
    def __init__(self, sensor: TemperatureSensor,
                 storage: DataStorage,
                 processor: SignalProcessor):
        self.sensor = sensor
        self.storage = storage
        self.processor = processor

    def acquire_data(self):
        measurement = self.sensor.read()
        processed = self.processor.process(measurement)
        self.storage.save(processed)
```

**If you find yourself thinking:**

- "It's like X but..."
- "It mostly works like X except..."
- "It's X with these limitations..."

→ You probably need composition, not inheritance.

## Practical Guidelines

The Liskov Substitution Principle says: **Subclasses must be substitutable for their base classes without breaking the program.**

Following LSP in scientific code:

- Ensures equipment/algorithms are truly interchangeable
- Prevents subtle bugs from "almost compatible" implementations
- Makes testing easier (test base class, trust subclasses)
- Clarifies design decisions (inheritance vs. composition)
- Documents contracts explicitly

**The key insight**: Inheritance creates a promise. The subclass must keep that promise completely, not just partially.

**Before creating a subclass, ask:**

1. Does the subclass **strengthen** the base class contract? ✅ OK
2. Does it **weaken** the contract in any way? ❌ Violates LSP
3. Does it change types, units, or conventions? ❌ Violates LSP
4. Would code written for the base class break with this subclass? ❌ Violates LSP

**If you answer "no" to question 1 or "yes" to questions 2-4, consider:**

- Using composition instead of inheritance
- Creating a separate class hierarchy
- Making the differences explicit in the type system

**Example: Does it weaken the contract?**

Base class: `process(data)` accepts any numpy array

Subclass: `process(data)` requires power-of-2 length

→ ❌ Violates LSP (strengthened precondition)

**Example: Does it change units?**

Base class: `get_distance()` returns meters

Subclass: `get_distance()` returns millimeters

→ ❌ Violates LSP (changed postcondition)

## When Pragmatism Trumps Perfect Substitutability

LSP is a guideline, not an absolute law. Sometimes you need to make trade-offs:

**Performance-critical code**: If a specialized subclass can be 10x faster by breaking strict substitutability, and the performance matters more than perfect polymorphism, document the limitation clearly and accept the trade-off.

**Prototyping and exploration**: When you're exploring a problem, strict LSP compliance can slow you down. Get it working first, refactor for substitutability later if you need multiple implementations.

**Legacy integration**: When wrapping third-party hardware or libraries that don't follow your contracts, sometimes an adapter that partially violates LSP is better than no integration at all.

The key: Make violations explicit, document them clearly, and refactor toward compliance when the code stabilizes.

## Performance Notes

Following LSP doesn't add performance overhead. The cost of method calls and inheritance is negligible compared to actual sensor reads, data processing, or I/O operations.

The real performance benefit: you can optimize one implementation without affecting others. Want a faster sensor class? Create one that follows the contract. No need to modify existing code.

## Your Turn

1. Find a place in your code where you use inheritance
2. Write tests for the base class (use the pattern shown in "Testing for LSP Compliance")
3. Run those tests on all subclasses
4. If any fail, you've found an LSP violation
5. Refactor to either fix the violation or remove the inheritance

In the next post, we'll explore the **Interface Segregation Principle**: why forcing classes to implement methods they don't need causes problems, and how to fix it.

---

*Have you been surprised by a subclass that didn't work like you expected? Share your story in the comments!*

**Previous posts in this series:**

- [Single Responsibility Principle for Scientists](../single_responsibility_principle)
- [Open/Closed Principle for Scientists](../open-closed_principle)

**Next in this series:**

- Interface Segregation Principle for Scientists - *Coming next week*
