# Networked Fingerprint Sensor Module (AS608, FPM10A) with ESP32 + Arduino and C#

# Networked Fingerprint Scanner with Arduino for AS608 and FPM10A fingerprint sensors.

The Networked Fingerprint Scanner provides solution for fast fingerprint enrollment and verification. It eliminates the issue of duplicate fingerprint entries and enable seamless integration and verification across multiple fingerprint scanner devices. This innovative system has been tested to support a range of fingerprint sensor modules, including AS608 and FPM10A, providing extensive compatibility over network connections.
Following extensive testing, the R305 sensor has been found incompatible with this system.



# Fingerprint Verification accross different Modules over Wifi Network
The fingerprint sensors, when integrated with our firmware, have been extensively tested to ensure compatibility and accurate fingerprint verification across networked devices. This means that fingerprints scanned on one device, for instance, comprising AS608 and ESP32, can be reliably matched with those scanned on another device, such as FPM10A or AS608. 
This is achieved with ESP32 wifi connected to a local network. The ESP32 communicates with C#-based central Server that stores fingerprint database.
## Connection to ESP32
<img width="768" alt="keypadpinconnection" src="https://github.com/jpdigitalman/C_RayFingerNetwork/assets/53490244/82ec2a64-af1a-4a92-92c7-b013e4bba611">
The above connection diagram shows how an OLED Display, 4X4 KEYPAD & Fingerprint Sensor is interfaced with ESP32 Board. The I2C pins of OLED Display, i.e SDA & SCL are connected to ESP32 PIN 23 and 22 pins respectively. Similarly, the fingerprint sensor MODULE is connected to UART pins TX (17) & RX (16). 

## Key Features of the sample firmware

- **Elimination of Duplicate Enrollment**: Prevention of redundant fingerprint entries with inbuilt fingerprint matching capabilities, ensuring each fingerprint is uniquely identified across all connected devices. The fingerprint matching uses the modules inbuilt matching capability to ensure consistent and reliable fingerprint identification.

- **Unlimited Fingerprint Storage**: With support for an unlimited number of fingerprints, our networked scanner allows for extensive user enrollment without any storage constraints. 

- **Seamless Integration**: Tested compatibility with various fingerprint sensors, such as AS608 and FPM10A, ensures smooth operation and reliable performance over network connections.

- **Real-time Verification**: Experience fast fingerprint verification across networked devices, enabling swift access control and enhanced security measures.
Note: The more fingerprint is stored in the remote server, the more time it takes to verify the fingerprint during enrollment. 
Test shows that 1000 fingerprints takes about 30 seconds to verify.
The benefit is that the fingerprint verification is not limited to a single machine.

- **Automatic Enrollment**: New fingerprints are automatically enrolled and saved in the database, streamlining the enrollment process for added convenience.

## How It Works

Our system operates by integrating multiple fingerprint sensors into a networked environment. By installing our proprietary sample firmware and configuring each sensor to connect to a Wi-Fi network using the ESP32's wireless capabilities, users can establish a seamless network of fingerprint scanners. These scanners communicate with our free open-source Central PC software, developed using C# .NET, which serves as the hub for storing fingerprint data in a transparent json database format and facilitating real-time verification.

# PC Software (Open Source)
To enable network verification, the fingerprint scanners communicate with PC software developed using C# .NET, which is licensed under the MIT License. This software stores fingerprint data in a database on the PC. Upon fingerprint input, the fingerprint module checks the each database to ensure uniqueness. If the fingerprint is not already stored, it's enrolled and saved in the database.

## Installation Steps

1. **Install Firmware**: Flash the provided firmware onto the ESP32 using the ESP flashing tool or a suitable method for flashing a (.bin) file.
   The sample firmwareis limited to run for 15mins or a maximum of 10 times. Which ever one comes first.

3. **Initial Setup**:
   - Power on the scanner for the first time. It will start in ACCESS POINT mode, displaying "Network setup attempt" on the OLED.
   Connect to the scanner's Wi-Fi using your PC. The Wi-Fi network details are as follows:
   SSID: C-Ray Foundation
   Password: password
   IP Address: 192.168.4.1
   Access the web interface to set up the Wi-Fi SSID and password for your router.
   Save the new network configuration.

4. **Connect to Wi-Fi**: Wait for the ESP32 fingerprint scanner to restart, automatically connecting to your Wi-Fi network. The OLED will display the scanner's new IP address upon successful connection.

5. **Install PC Software**: Compile the provided open-source C# code and run the PC software. It will display a user interface where you can input the scanner's IP address.

6. **Configure Windows Firewall**:
   - Add Port 3000 to TCP for both inbound and outbound rules in Windows Defender and Advanced Security. This allows communication between the PC software and ESP32.

7. **Submit Server IP Address**: Enter the IP address in the PC software's web address input and navigate to the server IP page. Click "submit" to save the server IP address in the ESP32 scanner's non-volatile memory.

8. **Final Setup**: The ESP32 scanner will restart and automatically connect to the server. Now, the scanner is ready for fingerprint enrollment and verification.

For access to the full code of the firmware, users can request it directly. Please note that the firmware code is not open source. However, the PC software written in C# is open source and available for customization.

## License Agreement

The Open source PC software component of this system is Free and licensed under the MIT License. For access to the full firmware code or other components not explicitly open-sourced, please reach out for further information.

The Networked Fingerprint Scanner firmware is provided under a proprietary license agreement. 
The ESP32 runs Arduino code.
For inquiries regarding licensing or access to the firmware code, please contact me.
