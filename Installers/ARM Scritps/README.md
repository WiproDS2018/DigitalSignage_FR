# Microsoft


# RMS Monitoring

**Table of Contents**

- [About Digital Signage](#about-digital-signage)
- [Architecture Diagram](#architecture-diagram)
- [Azure Services](#azure-services)
    - [Azure blob](#azure-blob)
    - [Azure IoT Hub ](#azure-iot-hub)
    - [Azure Web App](#azure-iot-hub)
    - [Azure Web Job](#azure-web-job)
    - [Azure SQL DB](#azure-sql-db)
- [Definitions](#definitions)
- [High Level Deployment Process](#high-level-deployment-process)
- [What is Azure Resource Manager(ARM) Template??](#what-is-azure-resource-manager(arm)-template??)
- [Deployment Costs](#deplyment-costs)
- [Pre-requisites](#pre-requisites)
- [ARM Template Input parameters](#arm-template-input-parameters)
- [Getting Started](#getting-started)
- [Deployment Scenario](#deployment-scenario)
    - [Deploy ARM templates using Azure portal](#deploy-arm-templates-using-azure-portal)
    - [Deploy ARM templates using  Azure CLI](#deploy-arm-templates-using-azure-cli)
- [Post Deployment](#post-deployment)
- [Solution Workflow:](#solution-workflow)
    - [Digital Player Signage](#digital-player-signage)
         - [Installation of Stick Player software on a Virtual machine](#installation-of-stick-player-software-on-a-virtual-machine)
    - [Signup to digital signage UI](#signup-to-digital-signage-ui) 
    - [Log in to stick virtual machine](#log-in-to-stick-virtual-machine)
    - [Validation](#validation)
         - [SQL Server verification](#sql-server-verification)
         - [Stick VM Cache Verification](#stick-vm-cache-verification)
    - [Monitoring components](#monitoring-components)
         - [Azure Application Insights](#azure-application-insights)    
         - [OMS log analytics](#oms-log-analytics)

## About Digital Signage

Digital Signage is a new way to create and display messaging campaigns to customers and employees throughout your organization, also called dynamic signage, you can remotely create, send and display messages to Televisions or other HDMI Capable Displays anywhere in your enterprise Wi-Fi network. 

With digital signage, we can get as creative as your imagination allows, and the more innovative your ad, the more attention it’s going to draw. Consider the following digital signage campaigns that have really went above and beyond to create a truly innovative customer experience.

## Architecture Diagram:

![alt text](https://github.com/sysgain/Wipro_DigitalSignage/raw/Monitoring/images/1%20.png)

## Azure Services:

The below described azure services are used for digital signage.

###  Azure blob

The word ‘Blob’ expands to Binary Large Object. Blobs include images, text files, videos and audios. There are three types of blobs in the service offered by Windows Azure namely block, append and page blobs.

-   **Block blobs** are collection of individual blocks with unique block ID. The block blobs allow the users to upload large amount of data.
-	**Append blobs** are optimized blocks that helps in making the operations efficient.
-	**Page blobs** are compilation of pages. They allow random read and write operations. While creating a blob, if the type is not specified they are set to block type by default.

All the blobs must be inside a container in your storage. 


###  Azure IoT Hub 

Azure IoT Hub is a fully managed service that enables reliable and secure bidirectional communications between millions of IoT devices and a solution back end. 
- Provides multiple device-to-cloud and cloud-to-device communication options. These options include one-way messaging, file transfer, and request-reply methods.
- Provides built-in declarative message routing to other Azure services.
- Provides a query able store for device metadata and synchronized state information.
- Enables secure communications and access control using per-device security keys. 
- Provides extensive monitoring for device connectivity and device identity management events.
- Includes device libraries for the most popular languages and platforms.

###  Azure Web App 

Azure Web Apps enables you to build and host web applications in the programming language of your choice without managing infrastructure. It offers auto-scaling and high availability, supports both Windows and Linux, and enables automated deployments from GitHub, Visual Studio Team Services.

###  Azure Web Job

Azure Web Job is back-end program you can run inside Azure, without Azure Web Job, you can deploy windows console app or windows service app to your server, then setup scheduler via windows scheduler or other third-party windows scheduler tools. 

###  Azure SQL DB

Azure SQL Database is a relational database-as-a service using the Microsoft SQL Server Engine. SQL Database is a high-performance, reliable, and secure database you can use to build data-driven applications and websites in the programming language of your choice, without needing to manage infrastructure.

## Definitions

### Players

Players are HDMI IoT sticks that are plugged into devices, like monitors, projectors or televisions, located at stations that scenes display on within a campaign.

### Player Group

Player Group is a collection of Players like group of displays in restaurant or mall.

### Stations

A station is a location like a hotel or restaurant with one or more players that scenes display on within a campaign.

### Scenes

Scenes are your messages, like billboards, that display on players located at stations within a campaign.

### Campaigns

A campaign is a group of stations, with one or more players at each station, that display one or more scenes on those players.

## High Level Deployment Process to be Followed

![alt text](https://github.com/sysgain/Wipro_DigitalSignage/raw/Monitoring/images/rms_blockdiagram.jpg)

## What is Azure Resource Manager(ARM) Template??

The Azure Resource Manager (ARM) is the service used to provision resources in your Azure subscription. It defines the set of resources (for example database server, database, and webapp) that is needed for a solution. ARM templates also specify deployment parameters that enable a user to configure settings for resources while requesting the resources.

## Deployment Costs

Below table describes the deployment costs per month for the solution. 

- Deployment Costs for Type1 (Azure services with free, Basic tier)

**Region – US West**


| S.NO | Resource Name            | Size  | Resource Costing model  | Azure Cost/Month|
| ---- |-------------    | --------  |-------------- | ---------------|
| 1    | Web Application | F1 (Free Tier), Shared Cores, 1 GB RAM, 1GB Storage   | PAYG | $0.00 |
| 2    | SQL Database       | B1 (Standard tier), 5DTU, 2GB included storage per DB | PAYG | $4.90 |
| 3    | IoTHub           | F1 (Free Tier), 500 devices, 8000 msgs/day  |  PAYG | $0.00 |
| 4    | Log Analytics (Optional)   | Free Tier, Daily limit: 500MB, Region East US | PAYG | $0.00  |
| 5    | Application Insights (Optional) |Basic, 1GB * $2.30, Region: West US2 | PAYG | $2.30 |
| 6    | Storage Account   | Block Blob Storage, General Purpose V1, LRS,100 GB Capacity  | PAYG |$2.40  |
|Total Cost |           |        |       | $7.30  |
|Total Cost Including Optional Components |      |     |      | $9.70  |


- Deployment Costs for Type2 

| S.NO | Resource Name            | Size  | Resource Costing model  | Azure Cost/Month|
| ---- |-------------     | --------  |--------------| ---------------|
| 1    | Web Application | B1 (1 Core, 1.75 GB RAM, 10 GB Storage)   | PAYG | $54.75  |
| 2    | SQL Database       |S0 (Standard tier), 10DTU, 250GB Storage | PAYG | $14.72 |
| 3    | IoTHub           | S1, Unlimited devices, 400,000 msgs/day,  |  PAYG | $50.00  |
| 4    | Log Analytics (Optional)   | Free Tier, Daily limit: 500MB, Region East US | PAYG | $0.00  |
| 5    | Application Insights (Optional) |Basic, 1GB * $2.30, Region: West US2 | PAYG | $2.30 |
| 6    | Storage Account   | Block Blob Storage, General Purpose V1, LRS Redundancy,100 GB Capacity    | PAYG |$2.40  |
|Total Cost |           |        |       | $121.43    |
|Total Cost Including Optional Components |      |     |      | $123.73    |

## Pre-requisites

1. GUID Generator
2. Stick devices (physical)

- GUID Generator
 
 The Azure runbook automate job needs a unique GUID as the jobID. We need to generate this GUID as a pre-requisite, please follow the steps to generate the Session Id or Job id, click on the link https://www.guidgenerator.com, which will redirect to the Online GUID Generator web page in a browser as shown below: 

 ![alt text](https://github.com/sysgain/Wipro_DigitalSignage/raw/Monitoring/images/10.png)

Click on Generate some GUIDs!  the results are displayed in the Results box as shown in the above figure.
Copy the GUID value, which will be used during the ARM template deployment


## ARM Template Input parameters:

Below is the list of Input parameters, that are to be provided as inputs to the ARM Template.

| S.NO | Parameter name                | Description            | Allowed values          | Default values                                                                                            
| ---- |---------------        | --------------------             | ------------         |-------------                                                                                               
| 1    |sqlAdministratorLogin                    | provide the user name for the sql server, please make a note of Username this will be used further |Any string      | Sqluser
| 2    |Storage Blob URL        | specify the blob url where all your code is present                          | Any string                    | https://projectiot.blob.core.windows.net/rms-iot/                                                      
| 3    |sqlAdministratorLoginPassword       | provide the password for the sql server, make a note of the Password this will be used further                     | Password must be 12 characters and have 3 of the following 1 lower case character, 1 number, and 1 special character                   | -
| 4    |Session id     |  The GUID Prefix for the runbook job to be started https://www.guidgenerator.com   |                    |
| 5   |Iot hub sku name         | specify the iot hub supported sku type F1,S1,S2,S3               |F1, S1, S2                    | F1
| 6   |             | The GUID Prefix for the runbook job to be started https://www.guidgenerator.com  | -                    | -
| 7    |Capacity units        | number of desired iot hub units. restricted to 1 unit for F1. Can be set up to maximum number allowed for subscription.                               | minValue: 1                   |1                                                            
| 8   |D2c message retention in days period | specify the iot hub messages retention period in days, for device-to-cloud messages                |minValue: 1, maxValue: 7                   |1
| 10    |D2c partition count | specify the number of desired partitions for device-to-cloud event ingestion. Restricted to 1 unit for F1                |1,2, 3, ….. 32                  |2
| 11    |Sku name | describes plan's pricing tier and instance size. check details at https://azure.microsoft.com/en-us/pricing/details/app-service/ https://azure.microsoft.com/en-us/pricing/details/app-service/                |            |
| 12    |Sku capacity | describes plan's instance count         |1              |1

## Getting Started

1. Login to git hub, navigate to your Digital Signage project repo, Select main-template. json

![alt text](https://github.com/sysgain/Wipro_DigitalSignage/raw/Monitoring/images/iot1.PNG)

2. Click on Raw

![alt text](https://github.com/sysgain/Wipro_DigitalSignage/raw/Monitoring/images/iot2.PNG)

3. Copy the raw template and paste in your azure portal for template deployment.

![alt text](https://github.com/sysgain/Wipro_DigitalSignage/raw/Monitoring/images/iot3.PNG)

## Deployment Scenario:

### Deploy arm template using Azure portal 

The Resource Manager template you deploy can either be a local file on your machine, or an external file that is in a repository like GitHub.

To deploy a template for Azure Resource Manager, follow the below steps.

1.	Open Azure portal, Navigate to New (+), search for the key word Template deployment.

![alt text](https://github.com/sysgain/Wipro_DigitalSignage/raw/Monitoring/images/2.png)

2.	The search results are displayed as shown in the following figure, Select Template deployment.

![alt text](https://github.com/sysgain/Wipro_DigitalSignage/raw/Monitoring/images/3.png)

3.	Click Create button as shown in the following figure.

![alt text](https://github.com/sysgain/Wipro_DigitalSignage/raw/Monitoring/images/4.png)

4.	Click Build your own Template in the editor link as shown in the following figure.

![alt text](https://github.com/sysgain/Wipro_DigitalSignage/raw/Monitoring/images/4.png)

5.	The edit template page is displayed as shown in the following figure.  

![alt text](https://github.com/sysgain/Wipro_DigitalSignage/raw/Monitoring/images/5.png)

6.	Replace / paste the template and click Save button.

![alt text](https://github.com/sysgain/Wipro_DigitalSignage/raw/Monitoring/images/6.png)

7.	The Custom deployment page is displayed as shown in the following figure.

![alt text](https://github.com/sysgain/Wipro_DigitalSignage/raw/Monitoring/images/7.png)

![alt text](https://github.com/sysgain/Wipro_DigitalSignage/raw/Monitoring/images/8.png)

8.	From Azure Portal, deploy the template by providing the following parameters in custom deployment settings.

9.	Paste the copied session id field in Settings section of the Custom deployment page as shown in below screen

![alt text](https://github.com/sysgain/Wipro_DigitalSignage/raw/Monitoring/images/a1.png)

![alt text](https://github.com/sysgain/Wipro_DigitalSignage/raw/Monitoring/images/a2.png)

10.	If you have already deployed IoT hub Sku as free tire in your subscription you will get an error, so please go with B1 from drop down. As shown in above screen.

11.	Once all the details are entered, select the I agree to the terms and conditions check box and click the Purchase button.

12.	After the successful deployment of the ARM template, the following resources are created in a Resource Group.
- App Service plan
- Automation Account
- Runbook
- 3 App Services
- Storage account
- IoT Hub
- IoT Device provisioning service
- SQL server
- SQL database
- Storage account
- Scheduler Job Collection

13.	Once the solution is deployed successfully navigate to the resource group, select on created resource group “rms_mon” then the following figure displays the list of resources that are created in the Resource Group.

![alt text](https://github.com/sysgain/Wipro_DigitalSignage/raw/Monitoring/images/13.png)
![alt text](https://github.com/sysgain/Wipro_DigitalSignage/raw/Monitoring/images/14.png)

## Deploy ARM Template using Cloud Shell

Azure CLI is used to deploy your resources to Azure. The Resource Manager template you deploy, can either be a local file on your machine, or an external file that is in a repository like GitHub. 

Azure Cloud Shell is an interactive, browser-accessible shell for managing Azure resources. Cloud Shell enables access to a browser-based command-line experience built with Azure management tasks in mind.

Deployment can proceed within the Azure Portal via Azure Cloud Shell.


### Customize main-template.parameters.json file

- Log in to the Azure portal
- Open the prompt.

![alt text](https://github.com/sysgain/Wipro_DigitalSignage/raw/Monitoring/images/15.png)

- Select Bash environment.

![alt text](https://github.com/sysgain/Wipro_DigitalSignage/raw/Monitoring/images/16.png)

- Select your preferred subscription from the dropdown list.

![alt text](https://github.com/sysgain/Wipro_DigitalSignage/raw/Monitoring/images/17.png)

- Copy main-template.json and main-template.parameters.json to your Cloud Shell before updating the parameters:
- Create main-template.json using the following command.

vim main-template.json

![alt text](https://github.com/sysgain/Wipro_DigitalSignage/raw/Monitoring/images/18.png)

- Paste your main-template.json in editor as shown below and save the file.

![alt text](https://github.com/sysgain/Wipro_DigitalSignage/raw/Monitoring/images/19.png)

- Create main-template.parameters.json.

![alt text](https://github.com/sysgain/Wipro_DigitalSignage/raw/Monitoring/images/20.png)

- Paste your main-template.parameters.json in editor.
-  Update the following parameters in main-template.json file
1. sqlAdministratorLogin
2. sqlAdminstratorLoginPassword
3. sessionId,storageBlobUrl 
4. omsWorkspaceRegion
5. omsAutomationRegion
6. dataRetention
7. omsLogAnalyticSku
8. appInsightsLocation
9. stickVMSize
10. iotHubSkuName
11.  capacityUnits
12. d2cMessageRetentionInDaysPeriod
13. d2cPartitionCount
14. skuName
15. skuCapacity
 
![alt text](https://github.com/sysgain/Wipro_DigitalSignage/raw/Monitoring/images/21.png)

- Use the az group create command to create a Resource Group in your region
**Description:**  To create a resource group, use az group create command,

It uses the name parameter to specify the name for resource group (-n) and location parameter to specify the location (-l).

**Syntax:** az group create -n <resource group name> -l <location>

az group create -n <****> -l <***>

![alt text](https://github.com/sysgain/Wipro_DigitalSignage/raw/Monitoring/images/22.png)

### Execute the template deployment

- Use the az group deployment create command to deploy the ARM template

**Description:** To deploy the ARM Template, you require two file:

1.	main-template.json – is contains the resource & its dependency resources to be provisioned from the ARM template

2.	main-template.parameters.json – is contains the input values that are required to provision respective SKU & Others details, For more 
details on what are the input parameter values navigate to Section 7 of this document.

**Syntax:** az group deployment create --template-file './<main-template.json filename>' --parameters '@./<main-template.parameters.json filename>' -g < provide resource group name that created in the section 8.1.2.2> -n deploy >> <provide the outputs filename>

az group deployment create --template-file './main-template.json' --parameters '@./main-template.parameters.json' -g rms-iot -n deploy >> outputs.txt

![alt text](https://github.com/sysgain/Wipro_DigitalSignage/raw/Monitoring/images/23.png)

- Deployment may take between 15-20 minutes depending on deployment size.
- After successful deployment you can see the values in the output section of ARM template for 
1. azureSQLEndpoint,
2. azureSQLsignageDBName
3. azureSQLUsername
4. azureSqlPassword
5. stcikVMFQDN
6. stcikVmadminUsername
7. stcikVmAdminPassword
8. destinationStorageAccountName
9. webJobUrl
10. IoTHubEndPoint 
11. webAppUrl
12. nodeServerUrl
13. omsLogAnalyticsUrl
14. appInsightsUrl
15. iotDpsGlobalEndPoint
16. iotDpsServerEndPoint
17. iotDpsIdScope

## Post Deployment

1.	After successful Arm Template deployment using azure portal, the output value can be obtained from Deployments > Microsoft.Template as shown in the following figure.

![alt text](https://github.com/sysgain/Wipro_DigitalSignage/raw/Monitoring/images/a3.png)

![alt text](https://github.com/sysgain/Wipro_DigitalSignage/raw/Monitoring/images/opt4.png)

## Solution Workflow:

## Signup to digital signage UI

### Installation of Stick Player software on a Virtual machine

1.	Download the Digital signage DPS player setup file from the below link
https://projectiot.blob.core.windows.net/rms-iot/DigitalSignageDpsPlayer.msi

![alt text](https://github.com/sysgain/Wipro_DigitalSignage/raw/Monitoring/images/a4.png)

2.	Click on more info, and select Run anyway.

![alt text](https://github.com/sysgain/Wipro_DigitalSignage/raw/Monitoring/images/a5.png)

3.	Click on Next

![alt text](https://github.com/sysgain/Wipro_DigitalSignage/raw/Monitoring/images/a6.png)

4.	Click on Next

![alt text](https://github.com/sysgain/Wipro_DigitalSignage/raw/Monitoring/images/a7.png)

5.	Click on Next

![alt text](https://github.com/sysgain/Wipro_DigitalSignage/raw/Monitoring/images/a8.png)

6.	You will get a dialog box, click on YES
7.	Click on Close

![alt text](https://github.com/sysgain/Wipro_DigitalSignage/raw/Monitoring/images/a9.png)

8.	Once installation is finished, navigate to path Local Disk (C:) > Program Files (x86) > Default Company Name > DigitalsignageDpsPlayer

![alt text](https://github.com/sysgain/Wipro_DigitalSignage/raw/Monitoring/images/a10.png)

9.	Right click on DigitalSignageDps.exe.config and open with notepad++

![alt text](https://github.com/sysgain/Wipro_DigitalSignage/raw/Monitoring/images/a11.png)

10.	Paste the copied ID Scope near DeviceProvisioningScopeId and URI near WebApiAddress, save the file.

![alt text](https://github.com/sysgain/Wipro_DigitalSignage/raw/Monitoring/images/a12.png)

11.	Navigate to path **Local Disk (C:) > Program Files (x86) > Default Company Name > DigitalsignageDpsPlayer**,
right click on DigitalSignageDps.exe file and run as Administrator, you can view Device ID as below screen. 

![alt text](https://github.com/sysgain/Wipro_DigitalSignage/raw/Monitoring/images/a13.jpg)

12.	The Digital Signage app displays the device id, &  shows Welcome page as below

![alt text](https://github.com/sysgain/Wipro_DigitalSignage/raw/Monitoring/images/a14.png)

13.	The Digital Signage app would create the device in Azure IoTHub with its device id

![alt text](https://github.com/sysgain/Wipro_DigitalSignage/raw/Monitoring/images/a15.png)

### Signup to digital signage UI

1. Copy and paste the following webAppUrl from the output section in a browser the following page is displayed.

![alt text](https://github.com/sysgain/Wipro_DigitalSignage/raw/Monitoring/images/a16.png)

2.	In the Sign in page, click New User? As shown in the following figure.

![alt text](https://github.com/sysgain/Wipro_DigitalSignage/raw/Monitoring/images/a17.png)

3.	The following new user sign in page is displayed.

![alt text](https://github.com/sysgain/Wipro_DigitalSignage/raw/Monitoring/images/a18.png)

4.	Enter all the details in the fields and click Submit button.
5.	A message “User registered successfully, please login!” is displayed as shown in following figure.

![alt text](https://github.com/sysgain/Wipro_DigitalSignage/raw/Monitoring/images/a19.png)

6.	Enter the details User id & Password in their respective fields.
7.	Once login you can view dashboard as shown in the following figure.

![alt text](https://github.com/sysgain/Wipro_DigitalSignage/raw/Monitoring/images/a20.png)

### Add Devices 
To add device, follow the below steps, 

1.	Click on “Device” from top of menu. 
    Select Add Device page is displayed as shown in the following figure.

![alt text](https://github.com/sysgain/Wipro_DigitalSignage/raw/Monitoring/images/b1.png)

1.	Enter Player Serial Number 
You must give the serial Number on the back of your what the device Id you are copied in notepad
 
2.	Enter Device Serial Number

You must give the serial Number on what the dps device Id created, you can copy it from resource groups > Iot Hub > Iot Devices from left side of panel > copy the Device Id.

![alt text](https://github.com/sysgain/Wipro_DigitalSignage/raw/Monitoring/images/b2.png)

3.	Paste the copied Device id in Device serial No and enter the name under Device Name respectively.

![alt text](https://github.com/sysgain/Wipro_DigitalSignage/raw/Monitoring/images/b3.png)

4.	Click Save button, the device is added.

![alt text](https://github.com/sysgain/Wipro_DigitalSignage/raw/Monitoring/images/b4.png)

**Note:** Here the Serial Number is Device Id created in IoT Hub ex: “digitalsignageiothubid” which was saved in earlier step.


### Device Group 

Select “Device” from the top menu, from the top menu select “Device Group”.

![alt text](https://github.com/sysgain/Wipro_DigitalSignage/raw/Monitoring/images/b5.png)

### Create Device Group 

Create a device group, Select Device Groups from the Device top menu and select “Create Device Group” from the left menu the following Add Device Group page is displayed.

![alt text](https://github.com/sysgain/Wipro_DigitalSignage/raw/Monitoring/images/b6.png)
![alt text](https://github.com/sysgain/Wipro_DigitalSignage/raw/Monitoring/images/b7.png)

1.	Enter the **Group Name** 
    This is the name for Device Group. You will find it in “attach Device/ Device Groups” in station. 
2.	Enter the **Group Description** 
     This is for describing the details of Device Group. 
3.	Click Save button. The newly added group is displayed as shown in the following figure.

![alt text](https://github.com/sysgain/Wipro_DigitalSignage/raw/Monitoring/images/b8.png)

### Assign a player to the player group

Once the device group is created, select the check box of the Group Name for which you want to assign the device, then select Assign Device from the left menu,

![alt text](https://github.com/sysgain/Wipro_DigitalSignage/raw/Monitoring/images/b9.png)

Once the Group name check box is selected the following page is displayed.

1.	Select Device(s) 
    Move the device by Drag-and-drop operation from the “List Available” to the “List Selected”. 
 
 ![alt text](https://github.com/sysgain/Wipro_DigitalSignage/raw/Monitoring/images/b10.png)

2.	Click Save button, the selected device is added to the group. 

![alt text](https://github.com/sysgain/Wipro_DigitalSignage/raw/Monitoring/images/b11.png)

To add Locations, Click Location from the top menu.

![alt text](https://github.com/sysgain/Wipro_DigitalSignage/raw/Monitoring/images/b12.png)

The Locations page is displayed as shown in the above figure.

### To Add Location 

Select “Add location” from the left Menu. 

![alt text](https://github.com/sysgain/Wipro_DigitalSignage/raw/Monitoring/images/c1.png)

1.	Enter Location Name 
This is the name of the location where the Device are located; hotel, restaurant, or other location. 
 
2.	Enter Location Details  
This is the general description information of the location. 
 
3.	Click Save button.

![alt text](https://github.com/sysgain/Wipro_DigitalSignage/raw/Monitoring/images/c2.png)

The location is added and displayed as shown in the following figure.

![alt text](https://github.com/sysgain/Wipro_DigitalSignage/raw/Monitoring/images/c3.png)

### Attach Device / Device Groups

Select one or more Location from the location List and then select “Attach Device / Device Group” from the left menu. You can now assign Device to the selected Locations(s). 

![alt text](https://github.com/sysgain/Wipro_DigitalSignage/raw/Monitoring/images/c4.png)

1. Select Player/Player Group(s) 

   Move players / Player Groups by Drag-and-drop operation from the “List Available” to the “List Selected”. 

2. Click Save button, the players are added to the station.

![alt text](https://github.com/sysgain/Wipro_DigitalSignage/raw/Monitoring/images/c5.png)

### Content

Select “Content” from the top menu. You can upload your own content, usually an image with text you have created, or you can select a Template and create your Content with the Template tool. 

![alt text](https://github.com/sysgain/Wipro_DigitalSignage/raw/Monitoring/images/c6.png)

### Add Content – Predefined Template  

Select “Add Content > manage > Predefined Template” in the left menu. This is your message composed of images and text that will appear on devices at your Content within your playlist. 

![alt text](https://github.com/sysgain/Wipro_DigitalSignage/raw/Monitoring/images/c7.png)

Choose a Template and select “Click to Edit and Upload.” As shown in the following figure. Choose a static Template to Customize with your images and text.  

![alt text](https://github.com/sysgain/Wipro_DigitalSignage/raw/Monitoring/images/c8.png)

Select the category from the dropdown list and enter the Template Name in the field and click Save button.

![alt text](https://github.com/sysgain/Wipro_DigitalSignage/raw/Monitoring/images/c9.png)

Click Save button the following page is displayed, then click Submit button.

![alt text](https://github.com/sysgain/Wipro_DigitalSignage/raw/Monitoring/images/c10.png)

The following submit content page is displayed.

![alt text](https://github.com/sysgain/Wipro_DigitalSignage/raw/Monitoring/images/c11.png)

Select View from the left menu to display the list of content that are added earlier.

![alt text](https://github.com/sysgain/Wipro_DigitalSignage/raw/Monitoring/images/c12.png)

Once the devices, groups, locations and contents are added then logout and login as administrator with Username: Admin, password: Admin

![alt text](https://github.com/sysgain/Wipro_DigitalSignage/raw/Monitoring/images/c13.png)

 Click Next the following Dashboard page is displayed.

 ![alt text](https://github.com/sysgain/Wipro_DigitalSignage/raw/Monitoring/images/c14.png)

 ### Admin 

 Select “Admin” from the top menu. In reports, you can manage “User Roles” and “Approvals” for your Campaigns. 

Go to Approvals Section and Click Approve button to approve the scene.

![alt text](https://github.com/sysgain/Wipro_DigitalSignage/raw/Monitoring/images/c15.png)

### Playlists 

Select “Playlist” from the top menu, the publish playlist page is displayed as shown in the following figure.

![alt text](https://github.com/sysgain/Wipro_DigitalSignage/raw/Monitoring/images/c16.png)

### To Add a Playlist

Select “Add Playlist” from the left menu, the Add Playlist page is displayed as shown in the following figure.

 ![alt text](https://github.com/sysgain/Wipro_DigitalSignage/raw/Monitoring/images/c17.png)

### Define the settings for the Playlist

1.	Select content to Preview and Publish 
Select one or more check boxes of the content for your Playlist. 
 
2.	Select a location from the dropdown list.
This is the location where your content will Play on. 
 
3.	Set the Start/End Date and Start/End Time 
 
4.	Set the Display Interval 
From the drop down menu select the display time interval ex : 3 sec. 
 
5.	Save the Playlist click on save.


![alt text](https://github.com/sysgain/Wipro_DigitalSignage/raw/Monitoring/images/c18.png)

Then the Playlist is added, click Publish button.

![alt text](https://github.com/sysgain/Wipro_DigitalSignage/raw/Monitoring/images/c19.png)

A message “Playlist Published Successfully”.
## Validation


### SQL Server verification

1. Go to the Azure portal > Resource group and click set server firewall.

![alt text](https://github.com/sysgain/Wipro_DigitalSignage/raw/Monitoring/images/64.png)

2.	Go to **Firewall settings > Add client IP**, Update the firewall rules and click Ok button.

![alt text](https://github.com/sysgain/Wipro_DigitalSignage/raw/Monitoring/images/65.png)

3.	Click Ok button a message “Success!” is displayed as shown in the following figure.

![alt text](https://github.com/sysgain/Wipro_DigitalSignage/raw/Monitoring/images/66.png)

4.	Open Azure portal and copy the server name.

![alt text](https://github.com/sysgain/Wipro_DigitalSignage/raw/Monitoring/images/67.png)

5.	Open SQL management server and paste the copied Server name in the respective field.
6.	Enter the Login and password which are provided for SQL at the time of template deployment and click Connect button

![alt text](https://github.com/sysgain/Wipro_DigitalSignage/raw/Monitoring/images/68.png)

7.	Navigate to **Databases > System Databases > Tables** and select the table as shown in the following figure.

![alt text](https://github.com/sysgain/Wipro_DigitalSignage/raw/Monitoring/images/69.png)

8.	Right click on dbo.devicecontent.tracker table and click Select  top 1000 rows.

![alt text](https://github.com/sysgain/Wipro_DigitalSignage/raw/Monitoring/images/70.png)

9.	The table is displayed as shown in the following figure.

![alt text](https://github.com/sysgain/Wipro_DigitalSignage/raw/Monitoring/images/71.png)

### Stick VM Cache Verification

1.	Go to Local disk (C:) > Device to verify deviceconfig.xml

![alt text](https://github.com/sysgain/Wipro_DigitalSignage/raw/Monitoring/images/a1.png)

2.	Go to Local Disk (C:) > Signage to verify

![alt text](https://github.com/sysgain/Wipro_DigitalSignage/raw/Monitoring/images/a2.png)

3.	Go to Local Disk (C:) > Signage > Images to verify the images

![alt text](https://github.com/sysgain/Wipro_DigitalSignage/raw/Monitoring/images/a3.png)

4.	Go to Local Disk (C:) > Signage > Config to check the config.xml

![alt text](https://github.com/sysgain/Wipro_DigitalSignage/raw/Monitoring/images/a4.png)

## Monitoring components

### Azure Application Insights

Application Insights is an extensible Application Performance Management (APM) service for web developers on multiple platforms. Use it to monitor your live web application. It will automatically detect performance anomalies. It includes powerful analytics tools to help you diagnose issues and to understand what users do with your app.

An overview of Application Insights, an Azure based service which makes it possible to monitor any application to know about its availability, failures and performance.

It works for apps on a wide variety of platforms including some of below.
-	.NET
-	Node.js
-	J2EE
-	hosted on-premises or in the cloud 

**What does Application Insights monitor?**

- Request rates, response times, and failure rates
- Dependency rates, response times, and failure rates
- Exceptions 
- Page views and load performance
- AJAX calls
- User and session counts
- Performance counters
- Performance counters
- Host diagnostics from Docker or Azure
- Diagnostic trace logs
- Custom events and metrics

### Application Insights 

1.	Go to **azure portal-->Resource Group-->RMS_monitoring-->Application Insights**
2.	Click on the **Application Insights icon** in the deployed resource group.
3.	You can see the below Server Response Time, Page View Load Time, Server Requests graphs.

![alt text](https://github.com/sysgain/Wipro_DigitalSignage/raw/Monitoring/images/92.png)

4.	To view the Server request Logs of web job, click on Server Requests.

![alt text](https://github.com/sysgain/Wipro_DigitalSignage/raw/Monitoring/images/93.png)

### Live Metrics Stream

Click on live metric stream to view the incoming requests, outgoing requests, overall health and servers of web application.

![alt text](https://github.com/sysgain/Wipro_DigitalSignage/raw/Monitoring/images/94.png)

![alt text](https://github.com/sysgain/Wipro_DigitalSignage/raw/Monitoring/images/95.png)

### Users

1.	Then click on users to view the number of users connected to the web application and here we can see, how many sessions are running in web application.

![alt text](https://github.com/sysgain/Wipro_DigitalSignage/raw/Monitoring/images/96.png)

2.	you can select the duration as per drop down list.

![alt text](https://github.com/sysgain/Wipro_DigitalSignage/raw/Monitoring/images/97.png)

3.	In this you also have other options like used, duration, by and split by in those you can set the metrics as per requirement.

![alt text](https://github.com/sysgain/Wipro_DigitalSignage/raw/Monitoring/images/98.png)

### Metric Explorer

1. Then select **metric explorer** from the left menu.
2. Then click on edit which appear on the right side top corner and it is shown in below figure.

![alt text](https://github.com/sysgain/Wipro_DigitalSignage/raw/Monitoring/images/99.png)

3.	After click on edit, on the right side you can see the chart details there you can set the details as per requirement.

![alt text](https://github.com/sysgain/Wipro_DigitalSignage/raw/Monitoring/images/100.png)

4.	After drag the chart details down, you can see the metrics and select the metrics as per requirement it is shown in below figure.

![alt text](https://github.com/sysgain/Wipro_DigitalSignage/raw/Monitoring/images/101.png)

5.	Similarly, we can get the metrics for server, client, failure, availability, performance counter and aggregated metrics also.

![alt text](https://github.com/sysgain/Wipro_DigitalSignage/raw/Monitoring/images/102.png)

6.	To check the logs, click on the chart of which you want to see the logs then you will get the logs of each request as shown in below figure.

![alt text](https://github.com/sysgain/Wipro_DigitalSignage/raw/Monitoring/images/103.png)

### Metric preview
1. Then select **metric explorer** from the left menu.
2.	Here you need to select the resource from the drop-down list, select the metric what you want to give and select the aggregation as per requirement.

![alt text](https://github.com/sysgain/Wipro_DigitalSignage/raw/Monitoring/images/104.png)

3.	Here we can see the graph after specifying our need.

![alt text](https://github.com/sysgain/Wipro_DigitalSignage/raw/Monitoring/images/105.png)

4.	User can usage customized query to view the metrics.
5.	Click on icon which is there in top of the right side of the overview panel.

![alt text](https://github.com/sysgain/Wipro_DigitalSignage/raw/Monitoring/images/106.png)

6.	After click on that icon, it will open a new tab with some default queries & chart for the same.

![alt text](https://github.com/sysgain/Wipro_DigitalSignage/raw/Monitoring/images/107.png)

7.	Then click on home page which is in left side of top menu and scroll down there are some default queries then click on run as per user requirement.

![alt text](https://github.com/sysgain/Wipro_DigitalSignage/raw/Monitoring/images/108.png)

8.	After click on performance, it will show some default queries & chart.

![alt text](https://github.com/sysgain/Wipro_DigitalSignage/raw/Monitoring/images/109.png)

### Application Map
1. Application Map helps you spot performance bottlenecks or failure hotspots across all components distributed application.
2. After click on application map you can see the screen like below.
3. Then Click on "Preview map" in the toggle at the top-right corner. You can use this toggle to switch back to the classic map.
4. When you click on Classic map, the application map will be.

![alt text](https://github.com/sysgain/Wipro_DigitalSignage/raw/Monitoring/images/110.png)

5.	When you click on preview map the application map will be like.

![alt text](https://github.com/sysgain/Wipro_DigitalSignage/raw/Monitoring/images/111.png)

## OMS log analytics

Operations Management Suite (also known as OMS) is a collection of management services that were designed in the cloud from the start. Rather than deploying and managing on-premises resources, OMS components are entirely hosted in Azure.
What is Log Analytics?
    Log Analytics is a service in Operations Management Suite (OMS) that monitors your cloud and on-premises environments to maintain their availability and performance. It collects data generated by resources in your cloud and on-premises environments and from other monitoring tools to provide analysis across multiple sources.
OMS are helpful to monitor SQL Database, Web Apps and Other Azure Components
In this Deployment we are using below two Solutions.
 - Azure SQL Analytics
 - Azure Web Apps Analytics

1. Go to azure portal-->resource group-->log analytics-->OMS portal and take the url.

![alt text](https://github.com/sysgain/Wipro_DigitalSignage/raw/Monitoring/images/112.png)

2.	Open OMS log analytics portal by clicking on the URL in the output section in same browser where user is already logged into azure portal.

![alt text](https://github.com/sysgain/Wipro_DigitalSignage/raw/Monitoring/images/113.png)

### Azure SQL Analytics

Azure SQL Analytics solution in Azure Log Analytics collects and visualizes important SQL Azure performance metrics.
The dashboard includes the overview of all databases that are monitored through different perspectives.

Selecting any of the tiles, opens a drill-down report into the specific perspective. Once the perspective is selected, the drill-down report is opened.

Using this Solution, we can find out below metrics
List of SQL Servers
List of Databases
Database Usage
Query Duration
Resource Utilization Like Storage, CPU percent, Deadlock and Physical data read
Etc.

1. You will click on the Azure SQL Analytics you can see this page.

![alt text](https://github.com/sysgain/Wipro_DigitalSignage/raw/Monitoring/images/114.png)

2.	Click on digitalsqlserverft5zx you will see the page like this.

![alt text](https://github.com/sysgain/Wipro_DigitalSignage/raw/Monitoring/images/115.png)

![alt text](https://github.com/sysgain/Wipro_DigitalSignage/raw/Monitoring/images/116.png)

### Azure Web Apps Analytics

The Azure Web Apps Analytics (Preview) solution provides insights into your Azure Web Apps by collecting different metrics across all your Azure Web App resources.
Using the solution, you can view the:
- Top Web Apps with the highest response time
- Number of requests across your Web Apps, including successful and failed requests
- Top Web Apps with highest incoming and outgoing traffic
- Top service plans with high CPU and memory utilization
- Azure Web Apps activity log operations
**Azure Web Apps metrics** like
- Average Memory Working Set
- Average Response Time
- Bytes Received/Sent
- CPU Time
- Requests
- Memory Working Set
- Httpxxx

1. Click on **Azure web apps**. 

![alt text](https://github.com/sysgain/Wipro_DigitalSignage/raw/Monitoring/images/117.png)

2. When you click on the Azure web apps analytics the below page will be shown

![alt text](https://github.com/sysgain/Wipro_DigitalSignage/raw/Monitoring/images/118.png)

3. We can drag the screen you will see the screen.

![alt text](https://github.com/sysgain/Wipro_DigitalSignage/raw/Monitoring/images/119.png)

### Log Search

log search to retrieve any data from Log Analytics. Whether you're analyzing data in the portal, configuring an alert rule to be notified of a condition, or retrieving data using the Log Analytics.

perform interactive analysis of data in the repository in the Azure portal or the Advanced Analytics portal.
create visualizations of data to be included in user dashboards with View Designer. Log searches provide the data used by tiles and visualization parts in each view.

1. Go to homepage Click on **log search** you will see the screen.

![alt text](https://github.com/sysgain/Wipro_DigitalSignage/raw/Monitoring/images/120.png)

2.	When you enter the search* you will see this screen.

![alt text](https://github.com/sysgain/Wipro_DigitalSignage/raw/Monitoring/images/122.png)

### IoT Hub

Navigate to page then You can enter this query.
search *
| where ( Type == "AzureMetrics" )
| search "DIGITAL-SIGNAGEHUBFT5ZX" you can see the page like this.

![alt text](https://github.com/sysgain/Wipro_DigitalSignage/raw/Monitoring/images/122.png)

### Analytics Page

Analytics portal is a web tool to write and execute Azure Log Analytics queries. perform interactive analysis of data in the repository in the Azure portal or the Advanced Analytics portal. Create visualizations of data to be included in user dashboards with View Designer. Log searches provide the data used by tiles and visualization parts in each view.

Click on **analytics** you can see this charts and tables.

![alt text](https://github.com/sysgain/Wipro_DigitalSignage/raw/Monitoring/images/123.png)

### Usage Page

Usage page provides the utilization of OMS Workspace. In Usage page, data volume utilization, Computers connected to workspace.

You can click on usage you will see the how much data is used for loged members.

![alt text](https://github.com/sysgain/Wipro_DigitalSignage/raw/Monitoring/images/124.png)
