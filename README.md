# Panels

Panels is a CLI tool that allows exporting a comic book in PDF format from a configuration file (XML, JSON, or YAML) and a set of images.

[![CI](https://github.com/samdouble/panels/actions/workflows/checks.yml/badge.svg)](https://github.com/samdouble/panels/actions/workflows/checks.yml)
[![Coverage Status](https://coveralls.io/repos/samdouble/panels/badge.svg?branch=master&service=github)](https://coveralls.io/github/samdouble/panels?branch=master)

[![C#](https://custom-icon-badges.demolab.com/badge/C%23-%23239120.svg?logo=cshrp&logoColor=white)](https://dotnet.microsoft.com/languages/csharp)
[![.NET](https://img.shields.io/badge/.NET-512BD4?logo=dotnet&logoColor=fff)](https://dotnet.microsoft.com/)
[![JSON](https://img.shields.io/badge/JSON-000?logo=json&logoColor=fff)](https://www.json.org/json-en.html)
[![XML](https://img.shields.io/badge/XML-767C52?logo=xml&logoColor=fff)](https://www.w3.org/XML/)
[![YAML](https://img.shields.io/badge/YAML-CB171E?logo=yaml&logoColor=fff)](https://yaml.org/)
[![NuGet](https://img.shields.io/badge/NuGet-004880?logo=nuget&logoColor=fff)](https://www.nuget.org/packages/Panels/)

## Installation

### Linux

**Debian/Ubuntu (.deb)**

Replace `VERSION` with the release version (e.g. `1.8.4`):

```
curl -sL -o Panels.VERSION-amd64.deb https://github.com/samdouble/panels/releases/download/vVERSION/Panels.VERSION-amd64.deb
sudo apt install "$(pwd)/Panels.VERSION-amd64.deb"
panels --help
```

On ARM64 Linux, use `Panels.VERSION-arm64.deb` instead.

The `.deb` installs a self-contained executable as `/usr/bin/panels` (lowercase). `/usr/bin` is already on your `PATH`, so no extra setup is needed.

If `apt install` reports an unsupported file, check the download with `file Panels.VERSION-amd64.deb` — it should say `Debian binary package`. If it shows HTML or plain text, the URL or filename was wrong and you need to re-download. You can also install with:

```
sudo dpkg -i Panels.VERSION-amd64.deb
```

**Portable zip (x64)**

```
curl -sL -o Panels.VERSION-linux-x64.zip https://github.com/samdouble/panels/releases/download/vVERSION/Panels.VERSION-linux-x64.zip
unzip Panels.VERSION-linux-x64.zip
./Panels --help
```

## How to Use

### Command-line Commands & Arguments

- `--version`: Show version information.

#### `generate`
This command will use the raw images and the config file to generate the final PDF file.

- `-c`, `--config` **(required)**: Path to the configuration file (`.xml`, `.json` or `.yaml`/`.yml`) describing the comic structure.
- `-i`, `--images` **(required)**: Path to the directory containing the images used in the comic.
- `-o`, `--output` *(optional)*: Path to the output PDF file. If not specified, the output will be named `output.pdf` in the current directory.
- `-?`, `-h`, `--help`: Show help information.

**Example:**

```
Panels -- generate -c path/to/config.xml -i path/to/images -o my_comic.pdf
```

#### `validate`
This command will validate the config file.

- `-c`, `--config` **(required)**: Path to the configuration file (XML, JSON, or YAML) to validate.
- `-?`, `-h`, `--help`: Show help information.

**Example:**

```
Panels -- validate -c path/to/config.xml
```

### Configuration

#### File Formats

Configuration is supported in three formats:

- **JSON** (`.json`) and **YAML** (`.yaml`, `.yml`): Same structure as JSON, with `$type` used for polymorphic nodes (e.g. `$type: slot`, `$type: panel`, `$type: text`, `$type: description`, `$type: newpage`). The root object is the comic; use a `children` array for slots and new pages.
- **XML** (`.xml`): Schema-validated; see [XML Configuration Reference](#xml-configuration-reference) below.
- **YAML** (`.yaml`, `.yml`): Same structure as JSON, with `$type` used for polymorphic nodes (e.g. `$type: slot`, `$type: panel`, `$type: text`, `$type: description`, `$type: newpage`). The root object is the comic; use a `children` array for slots and new pages.

#### Schema Reference

The XML configuration file defines the structure and content of your comic. Here is a minimal example:

```xml
<?xml version="1.0" encoding="UTF-8"?>
<comic
  version="1.0"
  marginTop="64"
  marginBottom="64"
  marginLeft="25"
  marginRight="25"
  horizontalPanelSpacing="10"
  verticalPanelSpacing="10"
  rowsPerPage="3"
>
  <slot>
    <panel image="0000.png">
      <text text="I love riding my bike!" />
      <description text="Character A is riding a bike." top="20" />
    </panel>
  </slot>
  <slot>
    <panel image="0010.png">
      <text text="Hi, how are you?" character="Alice" />
      <text text="I'm good. How about you?" character="Bob" top="20" />
    </panel>
  </slot>
</comic>
```

#### `<comic>`

The root element. Attributes:

- `version` (required): Version of the schema (e.g. "1.0").
- `bordersColor` (optional): Color for panel borders as a hex value (e.g. `#000000` for black). Defaults to black if neither comic nor panel specifies it. Can be overridden per panel with the panel's `bordersColor`.
- `bordersWidth` (optional): Stroke width in points for panel borders. Defaults to 2 if neither comic nor panel specifies it. Can be overridden per panel with the panel's `bordersWidth`.
- `fontSize` (optional, default: 12): Font size for text.
- `horizontalPanelSpacing` (optional): Horizontal spacing between panels in pixels.
- `marginTop` (optional): Margins in pixels.
- `marginBottom` (optional): Margins in pixels.
- `marginLeft` (optional): Margins in pixels.
- `marginRight` (optional): Margins in pixels.
- `rowsPerPage` (optional, default: 3): Number of rows per page.
- `showPageNumbers` (optional, default: true): When true, shows page numbers at the bottom center of each page. Set to false to hide them.
- `verticalPanelSpacing` (optional): Vertical spacing between panels in pixels.

A `<comic>` can contain multiple `<slot>` and `<newpage />` elements.

#### `<slot>`

Defines a column of panels. Attributes:

- `fontSize` (optional): Font size for the text in the slot.
- `maxPaddingLeft` (optional, default: 0): Maximum padding for panels in this slot.
- `maxPaddingRight` (optional, default: 0): Maximum padding for panels in this slot.

A `<slot>` can contain up to 2 `<panel>` elements.

#### `<panel>`

Represents a single panel in a slot. Attributes:

- `image` (required): Filename of the image for this panel.
- `borders` (optional): Set to `"none"` to hide the panel border. By default, panels have a black border.
- `bordersColor` (optional): Color for this panel's border as a hex value (e.g. `#FF0000` for red). Overrides the comic's `bordersColor` when both are set.
- `bordersWidth` (optional): Stroke width in points for this panel's border. Overrides the comic's `bordersWidth` when both are set.
- `fontSize` (optional): Font size for the text in the panel.
- `paddingBottom` (optional, default: 0): Padding from the bottom of the panel's image, in % of the image's height.
- `paddingTop` (optional, default: 0): Padding from the top of the panel's image, in % of the image's height.

A `<panel>` can contain multiple `<text>` and `<description>` elements.

#### `<text>`

Adds a text bubble or caption to a panel. Attributes:

- `text` (required): The text content.
- `character` (optional): Name of the character speaking.
- `fontSize` (optional): Font size for the text in the panel.
- `left` (optional, default: 0): Positioning of the text within the panel.
- `top` (optional, default: 0): Positioning of the text within the panel.
- `width` (optional, default: auto): Width of the text bubble in pixels.

#### `<description>`

Adds a description or narration to a panel. Attributes:

- `text` (required): The description content.
- `fontSize` (optional): Font size for the text in the panel.
- `left` (optional, default: 0): Positioning of the description.
- `top` (optional, default: 0): Positioning of the description.
- `visible` (optional, default: True): Controls visibility (e.g. "True" or "False").
- `width` (optional, default: auto): Width of the description bubble in pixels.

#### `<newpage />`

Forces a page break. Can be placed between slots.

## Development

### Running Locally

```
dotnet run --project Panels -- generate -c ~/Desktop/github_perso/bd/BD0/bd.xml -i ~/Desktop/github_perso/bd/BD0/images
```

or:

```
dotnet build && ./Panels/bin/Debug/net10.0/Panels generate -c ~/Desktop/github_perso/bd/BD0/bd.xml -i ~/Desktop/github_perso/bd/BD0/images
```

### Testing

```
dotnet test
```
