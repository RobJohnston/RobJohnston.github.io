+++
title = "One Class, One Job: Managing Scientific Code Complexity"
date = "2026-01-05"
publishDate = "2026-01-05"
draft = false
description = "Your spectroscopy script is now 2,000 lines and breaks when you change anything. Learn the Single Responsibility Principle with real scientific code examples."
category = "SOLID Principles"
tags = ["SRP"]
image = "/images/solid/single-responsibility.png"
aliases = ['/posts/solid-principles/single_responsibility_principle/']
+++

*Part 1 of the SOLID Principles for Scientific Programmers series*

## The Monolithic Monster

You started with a simple Python script to analyze some experimental data. A few hundred lines to load a CSV file, clean the data, run some statistics, and generate a plot. It worked perfectly!

Then your collaborator asked if you could add error bars. Then your advisor wanted the plots in a different format. Then you needed to support a different instrument's output format. Then someone found a bug in the outlier detection, and fixing it somehow broke the plotting code. Six months later, you have a 2,000-line file called `analysis.py` that nobody wants to touch because changing anything might break everything.

Sound familiar?

This is what happens when code lacks the **Single Responsibility Principle (SRP)**: the first and most fundamental of the SOLID principles.

## What Is the Single Responsibility Principle?

Robert C. Martin (Uncle Bob) originally stated it as:

> **A class should have only one reason to change.**  [^1]

He later clarified what he meant by 'reason to change'.  In a 2014 blog post, he stated:

> **Gather together the things that change for the same reasons. Separate those things that change for different reasons.**  [^2]

[^1]: *Martin, Robert C.*, [Agile Software Development: Principles, Patterns, and Practices](https://amzn.to/49mEjmD) (paid link). Pearson Education, 2003.
[^2]: *Martin, Robert C.*, "[The Single Responsibility Principle](https://blog.cleancoder.com/uncle-bob/2014/05/08/SingleReponsibilityPrinciple.html)". The Clean Code Blog, 8 May 2014.

More practically for scientists: Each piece of code should do one thing and do it well. When you need to modify your code, you should be able to pinpoint exactly where to make the change, and that change shouldn't have ripple effects throughout your entire codebase.

SRP applies equally well to procedural scripts. Even if you don't use classes, separating file loading, processing, plotting, and exporting into different modules or functions still yields the same benefits.

## Before You Refactor: Is It Worth It?

Before we dive into the solution, let's talk about when this is worth doing.  SRP takes time to implement, usually 10–30 minutes to extract a small responsibility into a function or class.  Don't refactor everything at once. Prioritize based on:

1. **How often does the code change?** High-change code benefits most from SRP
2. **How many people work on it?** Team code needs clearer boundaries
3. **How long will it be maintained?** Throwaway scripts don't need perfect structure
4. **Is it causing real pain?** If it works and nobody is suffering, maybe leave it alone

## A Real Example: The Problem

Let's look at what could be a typical spectroscopy analysis script.  Don't worry about understanding every detail—focus on the structure and the tangled responsibilities. These concepts apply to any object-oriented language.

```python
import pandas as pd
import numpy as np
import matplotlib.pyplot as plt
from scipy import signal, stats
import os

class SpectroscopyAnalysis:
    """Does everything for spectroscopy analysis."""

    def __init__(self, filename):
        self.filename = filename
        self.data = None
        self.processed_data = None
        self.peaks = None
        self.results = {}

    def run_complete_analysis(self):
        """Run the entire analysis pipeline."""

        # Load data from CSV
        print(f"Loading data from {self.filename}")
        try:
            self.data = pd.read_csv(self.filename, skiprows=2)
            # Handle different column names from different instruments
            if 'Wavelength' in self.data.columns:
                self.data.rename(columns={'Wavelength': 'wavelength',
                                         'Intensity': 'intensity'}, inplace=True)
            elif 'wl' in self.data.columns:
                self.data.rename(columns={'wl': 'wavelength',
                                         'int': 'intensity'}, inplace=True)
        except Exception as e:
            print(f"Error loading file: {e}")
            return None

        # Remove baseline
        print("Processing data...")
        baseline = np.polyfit(self.data['wavelength'],
                             self.data['intensity'], 2)
        baseline_curve = np.polyval(baseline, self.data['wavelength'])
        self.data['corrected'] = self.data['intensity'] - baseline_curve

        # Remove outliers
        z_scores = np.abs(stats.zscore(self.data['corrected']))
        self.data = self.data[z_scores < 3]

        # Smooth data
        self.processed_data = signal.savgol_filter(
            self.data['corrected'], 51, 3
        )

        # Find peaks
        print("Finding peaks...")
        peak_indices, properties = signal.find_peaks(
            self.processed_data,
            height=np.mean(self.processed_data) + 2*np.std(self.processed_data),
            distance=20
        )
        self.peaks = self.data.iloc[peak_indices]['wavelength'].values

        # Calculate statistics
        print("Calculating statistics...")
        self.results['mean_intensity'] = np.mean(self.processed_data)
        self.results['std_intensity'] = np.std(self.processed_data)
        self.results['num_peaks'] = len(self.peaks)
        self.results['peak_positions'] = self.peaks
        self.results['peak_intensities'] = self.processed_data[peak_indices]

        # Generate plots
        print("Generating plots...")
        fig, (ax1, ax2) = plt.subplots(2, 1, figsize=(10, 8))

        # Raw data plot
        ax1.plot(self.data['wavelength'], self.data['intensity'],
                'b-', label='Raw', alpha=0.5)
        ax1.plot(self.data['wavelength'], baseline_curve,
                'r--', label='Baseline')
        ax1.set_xlabel('Wavelength (nm)')
        ax1.set_ylabel('Intensity (a.u.)')
        ax1.legend()
        ax1.set_title(f'Analysis of {os.path.basename(self.filename)}')

        # Processed data plot
        ax2.plot(self.data['wavelength'], self.processed_data, 'g-')
        ax2.plot(self.peaks, self.processed_data[peak_indices],
                'ro', label='Peaks')
        ax2.set_xlabel('Wavelength (nm)')
        ax2.set_ylabel('Corrected Intensity (a.u.)')
        ax2.legend()

        plt.tight_layout()

        # Save plot
        output_filename = self.filename.replace('.csv', '_analysis.png')
        plt.savefig(output_filename, dpi=300)
        print(f"Plot saved to {output_filename}")

        # Save results to file
        results_filename = self.filename.replace('.csv', '_results.txt')
        with open(results_filename, 'w') as f:
            f.write("Spectroscopy Analysis Results\n")
            f.write("="*50 + "\n")
            f.write(f"File: {self.filename}\n")
            f.write(f"Number of peaks: {self.results['num_peaks']}\n")
            f.write(f"Mean intensity: {self.results['mean_intensity']:.2f}\n")
            f.write(f"Std intensity: {self.results['std_intensity']:.2f}\n")
            f.write("\nPeak positions (nm):\n")
            for i, (pos, intensity) in enumerate(zip(self.results['peak_positions'],
                                                     self.results['peak_intensities'])):
                f.write(f"  Peak {i+1}: {pos:.2f} nm (intensity: {intensity:.2f})\n")
        print(f"Results saved to {results_filename}")

        return self.results

# Usage
analysis = SpectroscopyAnalysis('sample_spectrum.csv')
results = analysis.run_complete_analysis()
```

This code works, but it's a nightmare to maintain. Let's count the responsibilities:

1. **File I/O**: Loading CSV files with different formats
2. **Data validation**: Handling missing columns, errors
3. **Baseline correction**: Polynomial fitting and subtraction
4. **Outlier removal**: Z-score filtering
5. **Smoothing**: Savitzky-Golay filter
6. **Peak detection**: Finding and characterizing peaks
7. **Statistical analysis**: Computing summary statistics
8. **Plotting**: Creating visualizations
9. **Report generation**: Saving results to text files

That's at least 9 different reasons this class might need to change!

## Why This Is a Problem

**Scenario 1**: Your collaborator needs to change the plot style (use different colors, add a legend in a different position).

*Problem*: The plotting code is tangled with data processing. You have to read through the entire method to find the plotting section.

**Scenario 2**: You want to use the same peak detection on a different type of data (Raman instead of infrared absorption).

*Problem*: The peak detection is hardwired to this specific class. You'd have to copy-paste the code or run the entire analysis pipeline.

**Scenario 3**: You need to support a new instrument's file format.

*Problem*: The file loading logic is mixed with column renaming and error handling, all in one giant method.

**Scenario 4**: A bug in the baseline correction is causing problems.

*Problem*: Fixing the baseline might accidentally break the plotting code because they're in the same method.

## The Solution: Single Responsibility Principle

Let's refactor this into classes that each have a single, well-defined responsibility:

⚠️
This example is intentionally large. Don't refactor everything at once. Start with the biggest pain points. SRP is iterative, not all-or-nothing.

```ascii
BEFORE:                    AFTER:
┌─────────────────────┐   ┌──────────┐  ┌───────────┐
│ SpectroscopyAnalysis│   │  Loader  │  │ Processor │
│ - loads             │   └──────────┘  └───────────┘
│ - validates         │   ┌──────────┐  ┌───────────┐
│ - processes         │   │  Peaks   │  │   Stats   │
│ - finds peaks       │   └──────────┘  └───────────┘
│ - plots             │   ┌──────────┐  ┌───────────┐
│ - saves             │   │ Plotter  │  │ Exporter  │
│ (9 responsibilities)│   └──────────┘  └───────────┘
└─────────────────────┘         ↓
                         ┌───────────────┐
                         │   Pipeline    │
                         │ (orchestrator)│
                         └───────────────┘
```

This is an illustrative refactor, not a prescription.  You don't need all of these classes immediately—this is the destination, not the starting point.  Real-world SRP often involves moving 50 lines of code at a time.

```python
import pandas as pd
import numpy as np
import matplotlib.pyplot as plt
from scipy import signal, stats
import os

# ------------------------------------------------------
# 1. Data Loading (Single Responsibility)
# ------------------------------------------------------
class SpectraLoader:
    def load(self, filename: str) -> pd.DataFrame:
        df = pd.read_csv(filename, skiprows=2)

        # Normalize column names
        if 'Wavelength' in df.columns:
            df = df.rename(columns={'Wavelength': 'wavelength',
                                   'Intensity': 'intensity'})
        elif 'wl' in df.columns:
            df = df.rename(columns={'wl': 'wavelength',
                                   'int': 'intensity'})
        return df


# ------------------------------------------------------
# 2. Preprocessing Pipeline (Baseline, Outliers, Smoothing)
# ------------------------------------------------------
class SpectraPreprocessor:
    def preprocess(self, df: pd.DataFrame) -> tuple[pd.DataFrame, np.ndarray, np.ndarray]:
        # Baseline removal
        baseline = np.polyfit(df['wavelength'], df['intensity'], 2)
        baseline_curve = np.polyval(baseline, df['wavelength'])
        df['corrected'] = df['intensity'] - baseline_curve

        # Outlier removal
        z = np.abs(stats.zscore(df['corrected']))
        df = df[z < 3]

        # Smoothing
        smoothed = signal.savgol_filter(df['corrected'], 51, 3)
        return df, baseline_curve, smoothed


# ------------------------------------------------------
# 3. Peak Detection
# ------------------------------------------------------
class PeakDetector:
    def detect(self, wavelengths: np.ndarray, smoothed: np.ndarray):
        peak_indices, _ = signal.find_peaks(
            smoothed,
            height=np.mean(smoothed) + 2*np.std(smoothed),
            distance=20
        )
        return peak_indices, wavelengths[peak_indices]


# ------------------------------------------------------
# 4. Statistics Computation
# ------------------------------------------------------
class SpectraStats:
    def compute(self, smoothed: np.ndarray, peak_indices, peak_positions):
        return {
            "mean_intensity": float(np.mean(smoothed)),
            "std_intensity": float(np.std(smoothed)),
            "num_peaks": len(peak_positions),
            "peak_positions": peak_positions,
            "peak_intensities": smoothed[peak_indices],
        }


# ------------------------------------------------------
# 5. Plotting Responsibility
# ------------------------------------------------------
class SpectraPlotter:
    def save_plot(self, filename, df, baseline_curve, smoothed, peaks, peak_indices):
        fig, (ax1, ax2) = plt.subplots(2, 1, figsize=(10, 8))

        # Raw data
        ax1.plot(df['wavelength'], df['intensity'], 'b-', alpha=0.5, label='Raw')
        ax1.plot(df['wavelength'], baseline_curve, 'r--', label='Baseline')
        ax1.legend()
        ax1.set_title(f"Analysis of {os.path.basename(filename)}")

        # Processed
        ax2.plot(df['wavelength'], smoothed, 'g-', label='Corrected')
        ax2.plot(peaks, smoothed[peak_indices], 'ro', label='Peaks')
        ax2.legend()

        plt.tight_layout()
        out = filename.replace(".csv", "_analysis.png")
        plt.savefig(out, dpi=300)
        return out


# ------------------------------------------------------
# 6. Results Saving Responsibility
# ------------------------------------------------------
class SpectraResultWriter:
    def save(self, filename: str, results: dict):
        out = filename.replace(".csv", "_results.txt")
        with open(out, "w") as f:
            f.write("Spectroscopy Analysis Results\n")
            f.write("="*50 + "\n")
            f.write(f"File: {filename}\n")
            f.write(f"Number of peaks: {results['num_peaks']}\n")
            f.write(f"Mean intensity: {results['mean_intensity']:.2f}\n")
            f.write(f"Std intensity: {results['std_intensity']:.2f}\n\n")
            f.write("Peak positions (nm):\n")
            for i, (pos, intensity) in enumerate(
                zip(results["peak_positions"], results["peak_intensities"])
            ):
                f.write(f"  Peak {i+1}: {pos:.2f} nm (intensity: {intensity:.2f})\n")
        return out


# ------------------------------------------------------
# 7. Orchestrator (Does NOT do analysis)
# ------------------------------------------------------
class SpectroscopyAnalysis:
    """
    Responsible ONLY for orchestration.
    Does not compute, plot, or load anything itself.
    """

    def __init__(self):
        self.loader = SpectraLoader()
        self.preprocessor = SpectraPreprocessor()
        self.detector = PeakDetector()
        self.stats = SpectraStats()
        self.plotter = SpectraPlotter()
        self.writer = SpectraResultWriter()

    def run(self, filename):
        df = self.loader.load(filename)

        df, baseline_curve, smoothed = self.preprocessor.preprocess(df)

        peak_indices, peak_positions = self.detector.detect(
            df['wavelength'].values, smoothed
        )

        results = self.stats.compute(smoothed, peak_indices, peak_positions)

        plot_file = self.plotter.save_plot(
            filename, df, baseline_curve, smoothed, peak_positions, peak_indices
        )

        results_file = self.writer.save(filename, results)

        results["plot_file"] = plot_file
        results["results_file"] = results_file
        return results


# Usage
analysis = SpectroscopyAnalysis()
results = analysis.run("sample_spectrum.csv")
```

## Why This Is Better

### 1. Easy to Modify Individual Components

**Want to change the plot style?** Only touch `SpectrumPlotter`:

```python
class CustomPlotter(SpectrumPlotter):
    """Custom plotting style for publications."""

    def plot_analysis(self, raw_spectrum, processed_spectrum, peaks, title):
        # Use your preferred style
        plt.style.use('seaborn-paper')
        fig, ax = plt.subplots(figsize=(6, 4))
        # Custom implementation
        return fig

# Use it
pipeline = SpectroscopyPipeline()
pipeline.plotter = CustomPlotter()  # Easy swap!
```

### 2. Easy to Reuse Components

**Want to use the peak finder on different data?**

```python
# Works with any Spectrum object!
finder = PeakFinder()
peaks = finder.find_peaks(my_raman_spectrum)
peaks = finder.find_peaks(my_xrd_pattern)
```

### 3. Easy to Test

Each component can be tested independently with synthetic data—no real files, no real instruments, no complex setup. For example:

```python
import unittest

class TestPeakFinder(unittest.TestCase):
    def test_finds_obvious_peak(self):
        # Create synthetic data with known peak
        wavelength = np.linspace(400, 700, 100)
        intensity = np.exp(-(wavelength - 550)**2 / 100)
        spectrum = Spectrum(wavelength, intensity)

        finder = PeakFinder()
        peaks = finder.find_peaks(spectrum)

        self.assertEqual(len(peaks), 1)
        self.assertAlmostEqual(peaks[0].position, 550, delta=5)
```

You can test the processor with known baselines, the loader with mock files, and the statistics with hand-calculated values. In the monolithic version, testing anything required the entire pipeline to work.

### 4. Easy to Extend

**Need to support a new file format?**

```python
class SpectrumLoader:
    def load_csv(self, filename: str) -> Spectrum:
        # existing code
        pass

    def load_json(self, filename: str) -> Spectrum:
        """New method for JSON files."""
        import json
        with open(filename) as f:
            data = json.load(f)
        return Spectrum(
            wavelength=np.array(data['wavelength']),
            intensity=np.array(data['intensity'])
        )
```

**Need a different baseline correction method?**

```python
class SpectrumProcessor:
    def remove_baseline(self, spectrum: Spectrum, poly_order: int = 2):
        # existing polynomial method
        pass

    def remove_baseline_als(self, spectrum: Spectrum,
                           lam: float = 1e5, p: float = 0.01):
        """Alternative: Asymmetric Least Squares baseline."""
        # New implementation
        pass
```

### 5. Clear Documentation and Understanding

Each class has a single, clear purpose:

- **SpectrumLoader**: "I load spectra from files"
- **SpectrumProcessor**: "I clean and process spectrum data"
- **PeakFinder**: "I find peaks in spectra"
- **StatisticalAnalyzer**: "I compute statistics"
- **SpectrumPlotter**: "I create plots"
- **ResultsExporter**: "I save results to files"

New collaborators can immediately understand what each piece does.

## Red Flags That You Need SRP

- Methods longer than 50 lines
- Changing one thing breaks another
- Can't write a test without mocking 5 dependencies
- Classes or functions with 'and' in their description
- You can't reuse a piece without bringing the whole system along
- The same code is repeated in multiple places with slight variations
- You're afraid to refactor

## Common Mistakes: Over-Engineering

The most common mistake I see is going too far. You don't need a separate class for every single function. Here's my rule of thumb:

**When to create a new class:**

- The responsibility is complex enough to warrant multiple methods
- You might want to swap implementations (different file formats, algorithms)
- The code could be reused in different contexts
- Testing would be easier with isolation

**When to keep it as a simple function:**

- It's a simple calculation or transformation
- It's only used in one place
- It has no internal state to manage
- It's obvious what it does

For example, computing a simple average doesn't need its own class—just use `np.mean()`. But if you're computing a suite of related statistics, a `StatisticalAnalyzer` makes sense.

## Performance Notes

You might worry about the overhead of creating multiple objects. In practice, this is negligible for typical scientific workflows. The real performance bottleneck is your algorithms (peak finding, smoothing), not object instantiation.

## Your Turn

Right now, open your largest script and identify the top two responsibilities that cause the most pain—usually file I/O or visualization.  Extract each into a function or small class. You've just started applying SRP.

Next week, we'll tackle the **Open/Closed Principle**: how to add new analysis methods without touching (and potentially breaking) your validated code.

---

*Wrestling with monolithic scientific code? Share your experiences in the comments!*
