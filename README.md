# InoaPS-Broker
InoaPS-Broker is an C# project for sending stock alerts to a list of emails.
## Installation
This project have dependencies, to install those run the following command.
```bash
dotnet build
```
## Configuration
This project need some credentials, make sure to insert those credentials on the "appsettings.json" file.
## Usage
**Using .NET Core:**
- The config file needs to be on the C# root project.
- Run the following command on the project folder.
```bash
dotnet run ${Stock_Symbol} ${Min_Price} ${Max_Price}
```
**Using executable:**
- The config file needs to be on the same directory of the executable.
- Run the following command on executable folder.

**Linux**
```bash
./InoaPS-Broker ${Stock_Symbol} ${Min_Price} ${Max_Price}
```

**Windows**
```bash
InoaPS-Broker.exe run ${Stock_Symbol} ${Min_Price} ${Max_Price}
```
