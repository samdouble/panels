# Panels

Panels is a CLI tool that allows you to export a comic book in PDF format from an XML configuration and a set of images.

## How to Use

### Command-line arguments

Panels accepts the following command-line arguments:

- `-c`, `--config` **(required)**: Path to the XML configuration file describing the comic structure.
- `-i`, `--images` **(required)**: Path to the directory containing the images used in the comic.
- `-o`, `--output` *(optional)*: Path to the output PDF file. If not specified, the output will be named `output.pdf` in the current directory.
- `--help`: Show help information.
- `--version`: Show version information.

**Example:**
```
dotnet run --project Panels -- -c path/to/config.xml -i path/to/images -o my_comic.pdf
```

### XML Configuration Reference

The XML configuration file defines the structure and content of your comic. Here is a minimal example:

```xml
<?xml version="1.0" encoding="UTF-8"?>
<comic
  version="1.0"
	topMargin="64"
	bottomMargin="64"
	leftMargin="25"
	rightMargin="25"
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
- `topMargin`, `bottomMargin`, `leftMargin`, `rightMargin` (optional): Margins in pixels.
- `horizontalPanelSpacing`, `verticalPanelSpacing` (optional): Spacing between panels in pixels.
- `fontSize` (optional, default: 12): Font size for text.
- `rowsPerPage` (optional, default: 3): Number of rows per page.

A `<comic>` can contain multiple `<slot>` and `<newpage />` elements.

#### `<slot>`

Defines a column of panels. Attributes:

- `maxCropLeft`, `maxCropRight` (optional, default: 0): Maximum cropping for panels in this slot.

A `<slot>` can contain up to 2 `<panel>` elements.

#### `<panel>`

Represents a single panel in a slot. Attributes:

- `image` (required): Filename of the image for this panel.

A `<panel>` can contain multiple `<text>` and `<description>` elements.

#### `<text>`

Adds a text bubble or caption to a panel. Attributes:

- `text` (required): The text content.
- `left`, `top` (optional): Positioning of the text within the panel.

#### `<description>`

Adds a description or narration to a panel. Attributes:

- `text` (required): The description content.
- `left`, `top` (optional): Positioning of the description.
- `visible` (optional): Controls visibility (e.g. "true" or "false").

#### `<newpage />`

Forces a page break. Can be placed between slots.

## Development

### Running Locally

```
dotnet run --project Panels -- -c ~/Desktop/github/proj/bd1/BD0/bd.xml -i ~/Desktop/github/proj/bd1/BD0/images
```

### Testing

```
dotnet test
```

### Releasing a New Version

1. Change the version number in the *csproj* file.
2. Merge into the `master` branch.
3. `dotnet-releaser` on CircleCI will create a new GitHub Release.

## Technologies & Languages

- C# 11
- .NET 8
- iText 8
- GitHub Actions
- GitHub Releases
