# Panels

[![CI](https://github.com/samdouble/panels/actions/workflows/checks.yml/badge.svg)](https://github.com/samdouble/panels/actions/workflows/checks.yml)
[![Coverage Status](https://coveralls.io/repos/samdouble/panels/badge.svg?branch=master&service=github)](https://coveralls.io/github/samdouble/panels?branch=master)

Panels is a CLI tool that allows you to export a comic book in PDF format from an XML configuration and a set of images.

## How to Use

### Command-line Commands & Arguments

- `--version`: Show version information.

#### `generate`
This command will use the raw images and the config file to generate the final PDF file.

- `-c`, `--config` **(required)**: Path to the XML configuration file describing the comic structure.
- `-i`, `--images` **(required)**: Path to the directory containing the images used in the comic.
- `-o`, `--output` *(optional)*: Path to the output PDF file. If not specified, the output will be named `output.pdf` in the current directory.
- `-?`, `-h`, `--help`: Show help information.

**Example:**
```
dotnet run --project Panels -- generate -c path/to/config.xml -i path/to/images -o my_comic.pdf
```

#### `validate`
This command will validate the config file.

- `-c`, `--config` **(required)**: Path to the XML configuration file describing the comic structure.
- `-?`, `-h`, `--help`: Show help information.

**Example:**
```
dotnet run --project Panels -- validate -c path/to/config.xml
```

### XML Configuration Reference

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
      <text text="Hi, how are you?" />
      <text text="I'm good. How about you?" top="20" />
    </panel>
  </slot>
</comic>
```

#### `<comic>`

The root element. Attributes:

- `version` (required): Version of the schema (e.g. "1.0").
- `fontSize` (optional, default: 12): Font size for text.
- `marginTop` (optional): Margins in pixels.
- `marginBottom` (optional): Margins in pixels.
- `marginLeft` (optional): Margins in pixels.
- `marginRight` (optional): Margins in pixels.
- `horizontalPanelSpacing` (optional): Horizontal spacing between panels in pixels.
- `verticalPanelSpacing` (optional): Vertical spacing between panels in pixels.
- `rowsPerPage` (optional, default: 3): Number of rows per page.

A `<comic>` can contain multiple `<slot>` and `<newpage />` elements.

#### `<slot>`

Defines a column of panels. Attributes:

- `maxCropLeft` (optional, default: 0): Maximum cropping for panels in this slot.
- `maxCropRight` (optional, default: 0): Maximum cropping for panels in this slot.

A `<slot>` can contain up to 2 `<panel>` elements.

#### `<panel>`

Represents a single panel in a slot. Attributes:

- `image` (required): Filename of the image for this panel.
- `cropBottom` (optional, default: 0): Cropping from the bottom of the panel's image, in % of the image's height
- `cropTop` (optional, default: 0): Cropping from the top of the panel's image, in % of the image's height

A `<panel>` can contain multiple `<text>` and `<description>` elements.

#### `<text>`

Adds a text bubble or caption to a panel. Attributes:

- `text` (required): The text content.
- `left` (optional, default: 0): Positioning of the text within the panel.
- `top` (optional, default: 0): Positioning of the text within the panel.
- `width` (optional, default: auto): Width of the text bubble in pixels.

#### `<description>`

Adds a description or narration to a panel. Attributes:

- `text` (required): The description content.
- `left` (optional, default: 0): Positioning of the description.
- `top` (optional, default: 0): Positioning of the description.
- `visible` (optional, default: True): Controls visibility (e.g. "True" or "False").
- `width` (optional, default: auto): Width of the description bubble in pixels.

#### `<newpage />`

Forces a page break. Can be placed between slots.

## Development

### Running Locally

```
dotnet run --project Panels -- generate -c ~/Desktop/github_perso/bd1/BD0/bd.xml -i ~/Desktop/github_perso/bd1/BD0/images
```

or:

```
dotnet build && ./Panels/bin/Debug/net7.0/Panels generate -c ~/Desktop/github_perso/bd1/BD0/bd.xml -i ~/Desktop/github_perso/bd1/BD0/images
```

### Testing

```
dotnet test
```

## Technologies & Languages

- C# 11
- .NET 7
- iText 9
- GitHub Actions
- GitHub Releases
