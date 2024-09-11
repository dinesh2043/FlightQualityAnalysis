# FlightQualityAnalysis
## Overview
FlightQualityAnalysis is a .NET 8 project that analyzes flight data stored in a CSV file located on an FTP server. The project utilizes FluentFTP for seamless FTP operations and follows best practices by storing sensitive information like FTP credentials in environment variables.
## Setup Instructions
### 1. Set Up FTP Server
The project was developed using an FTP server hosted locally on a PC. The following steps were used to configure the server:
- FileZilla Server was used to set up the FTP server.
- A specific user was created in the FTP server to grant access to the flight.csv file stored on the server.
### 2. File Access Permissions
- The user created in the FTP server has read access to the flight.csv file, which contains flight information in CSV format.
### 3. FTP Client with FluentFTP
- The project uses FluentFTP, a robust .NET library for interacting with FTP servers.
- FluentFTP’s FtpClient is used to connect to the FTP server, retrieve the flight.csv file, and process it for flight analysis.
### 4. Storing FTP Credentials
- FTP_HOST - The FTP server's address.
- FTP_USERNAME - The FTP user with access to the flight data.
- FTP_PASSWORD - The password for the FTP user.
### 5. Configuration
Ensure that the following environment variables are set before running the application:
´´´
FTP_HOST=your-ftp-host
FTP_USERNAME=your-ftp-username
FTP_PASSWORD=your-ftp-password
´´´
### 6. Example appsettings.json
You can configure the path to the flight CSV file in the appsettings.json:
´´´
{
  "FtpSettings": {
    "RemoteFilePath": "/flights.csv"
  }
}
´´´
### 6. Features
- FTP Integration: Connects to a local FTP server to download flight data.
- CSV Parsing: Uses CSVHelper to read and map flight data to a strongly-typed model (FlightInfo).
- Secure Configuration: Credentials are securely stored in environment variables, following best practices.
### 7. Prerequsits
- .NET 8 SDK: Ensure that you have .NET 8 installed.
- FileZilla Server (or any FTP server) for hosting the flight.csv file.
### 8. License
This project is only licensed for educational purposes.