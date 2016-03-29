# rsstowp_azure


Modified rsstowp for use as a WebJob in Azure.
###Instructions: 
* 1. Try posting to your site with rsstowp first.
* 2. Mod the App.config with the values you used in rsstowp (Keys.xml).
* 3. Publish as a Webjob in Azure from Visual Studio (for example create a new empty web app to house your scheduled webjobs).

App.config structure:
```xml
<?xml version="1.0" encoding="utf-8" ?>
<configuration>
  <configSections>
    <section name="userdata" type="System.Configuration.NameValueSectionHandler" />
  </configSections>
    <startup> 
        <supportedRuntime version="v4.0" sku=".NETFramework,Version=v4.5.2" />
    </startup>
  <userdata>
    <add key="ReaderToken" value="123456"/>
    <add key="FeedUrl" value="http://feeds.feedburner.com/crunchgear"/>
    <add key="WpBlogId" value="1"/>
    <add key="WpUrl" value="http://www.example.com"/>
    <add key="WpUser" value="wpuser"/>
    <add key="WpPassword" value="123456"/>
  </userdata>
</configuration>
```
