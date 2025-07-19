# SrtShifter

SrtShifter is a Windows Forms utility for shifting SRT subtitle timings. The program reads the duration of a selected MOV file and applies that offset to all entries in a chosen subtitle file.

## Features

- Parses MOV files to determine their duration using a custom parser.
- Loads and shifts SRT subtitle entries by the detected duration.
- Simple GUI that lets you select the video and subtitle files and then create a new `*_shifted.srt` file.

## Building

The application targets **.NET 8.0** and requires the .NET SDK. You can use the provided `dotnet-install.sh` script or install the SDK separately.

```bash
# restore dependencies and build
./dotnet-install.sh    # optional, installs dotnet if needed
 dotnet build SrtShifter.sln
```

### Building the Installer

The solution contains a WiX based setup project under `SrtShifterSetup`.
Build it to create an MSI installer:

```bash
dotnet build SrtShifterSetup/SrtShifterSetup.wixproj
```

The generated `SrtShifterSetup.msi` can then be used to install the
application on a Windows machine.

## Running

Run the program from the command line or start it from Visual Studio:

```bash
dotnet run --project SrtShifter
```

The main window allows you to browse for a `.mov` file and an `.srt` subtitle file. After processing, a new file named like `yourfile_shifted.srt` is created alongside the original subtitle file.

A small sample subtitle file is included at `SrtShifter/sample_file.srt` for testing.

## License

This project is licensed under the MIT License.

