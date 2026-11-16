# Doom test milestone

The first game test should be **classic Doom**, not a modern 3D game. It is a useful early benchmark because the original game is designed for very modest PC hardware.

## Test approach

1. First prove that the Android QEMU engine can start an x86 PC guest and keep it running.
2. Use a small DOS guest (for example FreeDOS) as a separate compatibility test profile. This does not replace the planned Windows 10 x64 guest.
3. Use a Doom-compatible engine and game data supplied by the user. Freedoom provides freely licensed replacement game data; do not bundle copyrighted commercial Doom WAD files or executable files in this repository.
4. Verify keyboard input, sound, display scaling, and guest disk persistence.
5. Only after the DOS test works, move to the heavier Windows 10 x64 guest.

## Why a separate DOS test?

Windows 10 x64 is a very heavy first boot target for a 4 GB Android phone when x86-64 CPU instructions must be translated in software. A DOS guest is a much smaller way to test the emulator's CPU, storage, display, and input pipeline before attempting Windows.

## Current status

This is a test plan only. The QEMU native engine is not integrated into URBEX PC yet, so no VM or Doom game can be launched from the current APK.
