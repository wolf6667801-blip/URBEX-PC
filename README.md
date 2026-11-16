# URBEX PC

URBEX PC is an experimental desktop shell inspired by Windows 7, built with C# Windows Forms.

## Current milestone

This repository starts with a Windows desktop prototype:
- Windows 7-inspired desktop layout
- Start menu and taskbar-style navigation
- Built-in file browser for the host computer
- Simple command console for basic shell commands
- About panel explaining the project's scope

> URBEX PC is **not** a Windows emulator or virtual machine. It is a desktop application/shell. Running Windows applications inside another operating system would require a separate compatibility layer or virtualization engine.

## Requirements

- Windows 7 Service Pack 1 or later
- .NET Framework 4.8
- Visual Studio 2019 or later with the .NET desktop development workload

## Build

1. Open `src/URBEX-PC.csproj` in Visual Studio.
2. Restore/build the project.
3. Run the resulting application.

For Windows 7, install the required Windows updates and .NET Framework 4.8 prerequisites.

## Project direction

The first goal is a stable desktop prototype. Android support and any real virtual-machine/compatibility engine are later, separate milestones.

## License

No license has been selected yet. All rights remain with the repository owner unless a license is added.
