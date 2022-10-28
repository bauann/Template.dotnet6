## Install project template

Jump into the folder which same location with sln file.
use the following command to install

```shell
dotnet new --install ./
```

## Uninstall project template

You can run anywhere with the following command

```shell
dotnet new --uninstall
```

After the command executed, console should display all the template that you have been installed.
And also display uninstall command that you can use.

## Install project template by nupkg file

### Generator nupke file
> follow the instruction from https://github.com/dotnet/templating/wiki/Runnable-Project-Templates

That says
> The whole contents of the project folder, together with the .template.config\template.json file, needs to be placed into a folder named content. 
<br>
Besides the content folder there needs to be a .nuspec file created. A packageTypes\packageType element with the value Template should be present in that file.
<br>
Both the content folder and the .nuspec file should reside in the same file system location.

so, we create a new .nuspec first. After created, it would be look like this

```xml
<?xml version="1.0" encoding="utf-8"?>
<package >
  <metadata>
    <id>pako.template.webapi</id>
    <version>1.0.0</version>
    <authors>victor</authors>
    <owners>victor</owners>
    <requireLicenseAcceptance>false</requireLicenseAcceptance>
    <license type="expression">MIT</license>
    <description>some description here</description>
    <releaseNotes>init release.</releaseNotes>
    <copyright>Copyright 2022</copyright>
    <tags>template</tags>
    <packageTypes>
      <packageType name="template" />
    </packageTypes>
  </metadata>
</package>
```
<i>Tips: you can run the following command to generator a default .nuspec file </i>
```bash
nuget spec
```

Put the <code>packageTypes</code> tag in <code>.nuspec</code> is very important. That means the package is a project type package.

Finally, we can generator nupakg file by excute the following command
```shell
nuget pack
```

### install project template by nupkg file

```shell
dotnet new --install PATH_OF_NUPKG
```

uninstall command
```shell
dotnet new --uninstall
```