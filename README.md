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

### XML Configuration

The XML configuration file defines the structure and content of your comic. Here is a minimal example:

```xml
<comic title="My Comic">
  <Page number="1">
    <Panel image="page1_panel1.jpg" x="0" y="0" width="400" height="600" />
    <Panel image="page1_panel2.jpg" x="400" y="0" width="400" height="600" />
  </Page>
  <Page number="2">
    <Panel image="page2_panel1.jpg" x="0" y="0" width="800" height="600" />
  </Page>
</comic>
```

- The root element is `<comic>`, with an optional `title` attribute.
- Each `<Page>` element represents a page in the comic book, with a `number` attribute.
- Each `<Panel>` element specifies an image and its position/size on the page using `x`, `y`, `width`, and `height` attributes.

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
