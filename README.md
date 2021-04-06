# LeseEulenBibliothek
Audiofile Management Tool for a TonUINO Project

## Features
- Creates a TonUINO compatible folder/file structure from a given source folder
- Automatic and manual numbering
- Persisting of structure data
- Incremental updates for new files and changed files without disturbing existing order
- Optional conversion from .wav or .ogg files into .mp3 format via a third party command line tool (tested with VLC media player http://www.videolan.org/)

## Requirements 
- .NetCore 3.1 Runtime

## Usage
1. Download/build and execute binary
2. Configure paths & converter settings (gears button - top right corner)
   - Archive: Folder with your named audio files (subfolders are expected) 
   - Output: Folder for TonUINO compatible folder/file structure
   - (Optional) Converter settings
3. Save configuration if changed
4. Run Scan
5. (optional) Do any manual numbering
6. Run Update
7. Copy files from the output folder to your TonUINO SD-Card
