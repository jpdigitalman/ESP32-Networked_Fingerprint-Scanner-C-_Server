# Networked Fingerprint Sensor Module (AS608, FPM10A) with ESP32 + Arduino and C#

# Networked Fingerprint Scanner with Arduino for AS608 and FPM10A fingerprint sensors.

The Networked Fingerprint Scanner offers a comprehensive solution for efficient fingerprint enrollment and verification, eliminating the hassle of duplicate fingerprint entries and enabling seamless integration and verification across multiple fingerprint scanner devices. This innovative system supports a range of fingerprint sensors, including AS608 and FPM10A, providing extensive compatibility over network connections.
Our solution has been meticulously tested and proven effective with supported sensors such as AS608 and FPM10A. It should be noted that, following extensive testing, the R305 sensor has been found incompatible with this system.

# What Does "Works Over Network" Mean?
The term "works over network" denotes that the fingerprint sensors, when integrated with our firmware, have been extensively tested to ensure compatibility and accurate fingerprint verification across networked devices. This means that fingerprints scanned on one device, for instance, comprising AS608 and ESP32, can be reliably matched with those scanned on another device, such as FPM10A or AS608.

## Key Features

- **Elimination of Duplicate Enrollment**: Say goodbye to redundant entries with our system's advanced fingerprint matching capabilities, ensuring each fingerprint is uniquely identified across all connected devices.

- **Unlimited Fingerprint Storage**: With support for an unlimited number of fingerprints, our networked scanner allows for extensive user enrollment without any storage constraints.

- **Seamless Integration**: Tested compatibility with various fingerprint sensors, such as AS608 and FPM10A, ensures smooth operation and reliable performance over network connections.

- **Real-time Verification**: Experience instant fingerprint verification across networked devices, enabling swift access control and enhanced security measures.

- **Automatic Enrollment**: New fingerprints are automatically enrolled and saved in the database, streamlining the enrollment process for added convenience.

## How It Works

Our system operates by integrating multiple fingerprint sensors into a networked environment. By installing our proprietary firmware and configuring each sensor to connect to a Wi-Fi network using the ESP32's wireless capabilities, users can establish a seamless network of fingerprint scanners. These scanners communicate with our free open-source Central PC software, developed using C# .NET, which serves as the hub for storing fingerprint data and facilitating real-time verification.

# PC Software (Open Source)
To enable network verification, the fingerprint scanners communicate with PC software developed using C# .NET, which is licensed under the MIT License. This software stores fingerprint data in a database on the PC. Upon fingerprint input, the system checks the database to ensure uniqueness. If the fingerprint is not already stored, it's enrolled and saved in the database.

## Installation Steps

1. **Install Firmware**: Flash the provided firmware onto the ESP32 using the ESP flashing tool or a suitable method for flashing a bin file.

2. **Initial Setup**:
   - Power on the scanner for the first time. It will start in ACCESS POINT mode, displaying "Network setup attempt" on the OLED.
   Connect to the scanner's Wi-Fi using your PC. The Wi-Fi network details are as follows:
   SSID: C-Ray Foundation
   Password: password
   IP Address: 192.168.4.1
   Access the web interface to set up the Wi-Fi SSID and password for your router.
   Save the new network configuration.

3. **Connect to Wi-Fi**: Wait for the ESP32 fingerprint scanner to restart, automatically connecting to your Wi-Fi network. The OLED will display the scanner's new IP address upon successful connection.

4. **Install PC Software**: Compile the provided open-source C# code and run the PC software. It will display a user interface where you can input the scanner's IP address.

5. **Configure Windows Firewall**:
   - Add Port 3000 to TCP for both inbound and outbound rules in Windows Defender and Advanced Security. This allows communication between the PC software and ESP32.

6. **Submit Server IP Address**: Enter the IP address in the PC software's web address input and navigate to the server IP page. Click "submit" to save the server IP address in the ESP32 scanner's non-volatile memory.

7. **Final Setup**: The ESP32 scanner will restart and automatically connect to the server. Now, the scanner is ready for fingerprint enrollment and verification.

For access to the full code of the firmware, users can request it directly. Please note that the firmware code is not open source. However, the PC software written in C# is open source and available for customization.

## License Agreement

The Open source PC software component of this system is licensed under the MIT License. For access to the full firmware code or other components not explicitly open-sourced, please reach out for further information.

The Networked Fingerprint Scanner firmware is provided under a proprietary license agreement. 

For inquiries regarding licensing or access to the firmware code, please contact [].
