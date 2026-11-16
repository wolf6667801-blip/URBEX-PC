# URBEX PC roadmap

## Direction: Android app + real Windows 10 x64 guest

- [x] Create repository and initial desktop-shell mock-up
- [x] Choose Windows 10 x64 as the intended guest OS
- [x] Define persistent virtual-disk requirement
- [ ] Build an Android ARM64 app shell
- [ ] Integrate/build a maintained QEMU Android engine
- [ ] Create and configure a persistent QCOW2 disk
- [ ] Let the user select a Windows 10 ISO and provide their own license
- [ ] Boot-test Windows 10 x64 on real target hardware
- [ ] Add clean shutdown, pause/resume, and VM status
- [ ] Add backup/export and snapshot support
- [ ] Add optional XP-inspired desktop customization instructions
- [ ] Measure RAM, CPU load, heat, and boot time on low-memory devices

## Non-negotiable engineering constraints

- A visual Windows-like shell is not a virtual machine.
- Android ARM64 running an x86-64 guest will likely need slow software CPU translation.
- A 4 GB phone may not have enough free memory for a useful Windows 10 x64 session.
- Never claim Steam or games work until tested individually.
- Never bundle Windows ISO files or product keys.

The first real milestone is a proof of concept that boots a user-provided Windows 10 x64 ISO and preserves guest changes on a QCOW2 disk. If this cannot run acceptably on the target phone, document that honestly and consider remote desktop to a separate PC as the practical fallback.
