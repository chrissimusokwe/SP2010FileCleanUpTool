## Deployment Guide - SharePoint 2010 File / Version History Clean Up Tool

A tool for cleaning up version history that has span out of control and
resulted in large size document libraries (At the time, SharePoint 2010
had no shredded storage, therefore, each version retained the full
document size). The tool is packaged as a WSP and the clean up happens
as a TimerJob, activated as a farm feature while the parameters like how
many versions to retain or whether a site collection should be skipped
for clean up is activated as a site collection feature.


**Assumptions and Prerequisites**

The following assumptions and prerequisites have been made


1.	The user deploying the WSP has farm administrator privileges

2.	The Farm is a SharePoint Server 2010 implementation.

3.	At this point, only files of content type “Document” are cleaned up.

4.	The following document libraries have been ignored for clean-up.

  •	Master Page Gallery

  •	Form Templates

  •	Site Assets

  •	Site Collection Documents

  •	Style Library

  •	Site Pages

  •	Customized Reports

  •	Site Collection Images

  •	and all hidden document libraries



**Deployed Artefacts**

The following artefacts are packaged in the solution

1.	SPFileCleanUp.TimerJob feature, as a Web Application feature

2.	SPFileCleanUp.Site feature, as a Site Collection feature

3.	PropertyBagsSettings.aspx, as a custom page accessed at Site Collection Administration and stored in the 14 hive as /_layouts/SPFileCleanUp/PropertyBagsSettings.aspx



**Uploading and deploying the WSP file**

The following steps are to be taken in order to add the WSP solution file to the Solution store and deploy to the relevant site collections.

1.	On the command line, assuming that the WSP has been copied to the root of c drive, run the command as seen in the screen dump below. 
![uploading wsp 1](https://cloud.githubusercontent.com/assets/12210489/19414448/19abb12c-93aa-11e6-9d6a-70a2cc33897e.png)

2.	After successfully adding to the Solution Store, the solution should be visible as seen below.
![uploading wsp 2](https://cloud.githubusercontent.com/assets/12210489/19414449/19b3db68-93aa-11e6-8bf5-b3b6f77a8b7d.png)

3.	Now click on the solution name to deploy it, and page as seen below will come up.
![uploading wsp 3](https://cloud.githubusercontent.com/assets/12210489/19414450/19ba0b00-93aa-11e6-8bd6-a4bfbcebe9e2.png)

4.	Note that the solution deploys globally, please click “OK” to deploy.

5.	Once deployed successfully, it should show as seen below.
![uploading wsp 5](https://cloud.githubusercontent.com/assets/12210489/19414451/19be8b76-93aa-11e6-8c35-399f38be0bf1.png)

6.	The uploading and deployment of the WSP is now done.


## Activation of features

The following Steps are to be taken, to activate the features at the site collection level that requires file clean up, and at its parent web application level.

**Site Collection feature activation

1.	Open the site collection that you require to set clean up.

2.	Click Site Actions > Site Settings, under Site Collection Administration, click Site Collection Features. 
![act sc 2](https://cloud.githubusercontent.com/assets/12210489/19414596/6e20132a-93ae-11e6-89a2-4eaa48981f67.png)

3.	Scroll to locate the feature “SPFileCleanUp.Site” as seen below ![act sc 3](https://cloud.githubusercontent.com/assets/12210489/19414597/6e28978e-93ae-11e6-9683-a6fb7a35b3b5.png)


4.	Click “Activate”, and once successfully activated it will show as seen below ![act sc 4](https://cloud.githubusercontent.com/assets/12210489/19414598/6e2980fe-93ae-11e6-9d0b-561cc1823455.png)

5.	Click Site Actions > Site Settings, under Site Collection Administration, you will now have an option at the bottom called “Property Bag Custom Settings” ![act sc 5](https://cloud.githubusercontent.com/assets/12210489/19414599/6e2ce5fa-93ae-11e6-83ad-34c10ee49910.png)

6.	Click “Property Bag Custom Settings” and a custom page will open up, showing site collection properties, as seen below ![act sc 6](https://cloud.githubusercontent.com/assets/12210489/19414600/6e2ec76c-93ae-11e6-8552-a7adf9cf0914.png)

7.	DO NOT delete nor modify any of the already existing property keys.

8.	Two property keys will need to be inserted and will be used by the SPFileCleanUp tool. Without these keys being inserted, the SPFileCleanUp will never run.

9.	The first property key to be inserted will be a key that indicates if the site collection is marked for clean up or not.

10.	 Using the text field Key, type in lowercase “spfilecleanup”, and the Value one as a digit “1”, as seen below, and click “Insert” ![act sc 10](https://cloud.githubusercontent.com/assets/12210489/19414601/6e2ef994-93ae-11e6-98cf-46c0a5d1298c.png)

11.	A value of “0” would indicate that the site collection should be skipped for file clean up.

12.	Once the insert is successful, the property key should now be visible as seen below in the list of existing property keys ![act sc 12](https://cloud.githubusercontent.com/assets/12210489/19414602/6e48c2a2-93ae-11e6-9431-150c698e2e7f.png)

13.	The second property key to be inserted will be a key that indicates the total count of file previous versions that should be returned in a file’s SharePoint version history.

14.	Using the text field Key, type in lowercase “keepversions”, and the Value as a digit “3”, to keep three previous versions, as seen below, and click “Insert” ![act sc 14](https://cloud.githubusercontent.com/assets/12210489/19414603/6e4f615c-93ae-11e6-8efb-713c9062a6c0.png)  

15.	Once the insert is successful, the property key should now be visible as seen below in the list of existing property keys ![act sc 15](https://cloud.githubusercontent.com/assets/12210489/19414604/6e52d1f2-93ae-11e6-80f2-998cd62a5c6d.png) 

16.	The above values of the property keys “spfilecleanup”  and “keepversions” can be edited at any later stage, depending on the site collection requirements, by the site collection administrator.

17.	The above steps of activating and configuring at Site Collection would have to be repeated for every site collection that requires clean up.



**Web Application feature Activation**

1.	Open Central Administration site

2.	Under Application Management section, click “Manage Web Applications”

3.	Highlight the Web Application that you wish to schedule file clean up on, as seen below.

4.	On the ribbon, under Manage section, click “Manage features”

5.	Scroll to locate the feature “SPFileCleanUp.TimerJob” as seen below.

6.	Click “Activate” to activate the web application feature, and once successfully activated, it should show as seen below.

7.	While on Central Administration, navigate to “Monitoring” page.  

8.	Under Timer Jobs sections, click “Review job definitions”, and scroll to locate the job definition called “Clean Up Schedule Timer Job”. 

9.	Click the job definition “Clean Up Schedule Timer Job” to edit it and the page shown below will come up with default settings.  

10.	Change the scheduled time to run the job according to your requirement, in our case, we schedule it to run daily, starting at 6PM but no later than 7PM, as seen below  

11.	The above steps for Web Application feature activation will have to be repeated for every Web Application that requires file clean up.



## Event Logging

The SPFileCleanUp tool writes to the windows event logs the following activities:

1.	The total count of versions per file that have been cleaned up.

2.	The file name cleaned up

3.	The time the file was cleaned up

4.	Any errors it may come across during the clean-up process.
 
