# Simulation of Mobile Medicine Administration System for Hospitals

The Mobile Medicine Administration System is a project study conducted by the following Electronics Engineering students of FEU Institute of Technology:

- Jann Raphael P. Alcoriza
- Jonas Edwin C. Fonacier
- Glenn Howard C. Paredes
- Charlemagne Zoilo C. Rivera
- Joven E. Subala

## Repository structure

This repository is divided into four folders:

1. **TPT-MMAS.IMSDatabase** - This is the Laravel-based web application that runs the API that talks with the IMS database and returns a JSON-formatted output.
2. **TPT-MMAS.SimulatedHospitalDatabase** - This is the Laravel-based web application simulates the API running on an existing hospital database setup. It also returns a JSON-formatted output.
3. **TPT-MMAS.TrayController** - This contains the C-based Arduino Wiring code that runs on the custom tray controller.
1. **TPT-MMAS.Windows10** - This contains the solution file that consists of the In-patient Management System client that connects to the APIs from `TPT-MMAS.IMSDatabase`. It also has the MMAS application running at the Windows 10 IoT Core installation on the Raspberry Pi 2.