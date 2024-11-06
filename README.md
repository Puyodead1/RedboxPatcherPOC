# Redbox Kiosk Patcher

- Launcher - A wrapper around kioskengine.exe that loads Harmony and Harmony patches
- CustomPatches - My own patches for personal stuff
- RedboxPatches - The main patch set


## Setup
You will need to create a `libs` folder in the root of the project, this is where any DLLs that are being patched will need to be so they can be referenced in the patch project

For example:
```
RedboxPatcher/
├─ libs/
│  ├─ Redbox.Rental.Services.dll
```


# Build
Use `Build Solution`, all output will be in the `build` folder, and should already be in the correct folder structure

# Usage
- Go to the Kiosk Engine bin folder (`C:\Program Files\Redbox\REDS\Kiosk Engine\bin`) and copy all files from `build` into it
- Go to the Kiosk Shell folder (`C:\Program Files\Redbox\KioskShell`) and open `KioskShell.exe.config` in a text editor, scroll down and find the following:
```
<setting name="KioskEnginePath" serializeAs="String">
	<value>C:\Program Files\Redbox\REDS\Kiosk Engine\bin\kioskengine.exe</value>
</setting>
```
Change the exe from `kioskengine.exe` to `Launcher.exe`, save and close the file