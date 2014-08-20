<?xml version="1.0" encoding="utf-8"?>
<serviceModel xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema" name="AudibleGlobeAPI" generation="1" functional="0" release="0" Id="8c16f284-78e7-4731-874b-7f1e53d9b988" dslVersion="1.2.0.0" xmlns="http://schemas.microsoft.com/dsltools/RDSM">
  <groups>
    <group name="AudibleGlobeAPIGroup" generation="1" functional="0" release="0">
      <componentports>
        <inPort name="AudibleGlobeAPIWorkerRole:Endpoint1" protocol="http">
          <inToChannel>
            <lBChannelMoniker name="/AudibleGlobeAPI/AudibleGlobeAPIGroup/LB:AudibleGlobeAPIWorkerRole:Endpoint1" />
          </inToChannel>
        </inPort>
      </componentports>
      <settings>
        <aCS name="AudibleGlobeAPIWorkerRole:Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" defaultValue="">
          <maps>
            <mapMoniker name="/AudibleGlobeAPI/AudibleGlobeAPIGroup/MapAudibleGlobeAPIWorkerRole:Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" />
          </maps>
        </aCS>
        <aCS name="AudibleGlobeAPIWorkerRoleInstances" defaultValue="[1,1,1]">
          <maps>
            <mapMoniker name="/AudibleGlobeAPI/AudibleGlobeAPIGroup/MapAudibleGlobeAPIWorkerRoleInstances" />
          </maps>
        </aCS>
      </settings>
      <channels>
        <lBChannel name="LB:AudibleGlobeAPIWorkerRole:Endpoint1">
          <toPorts>
            <inPortMoniker name="/AudibleGlobeAPI/AudibleGlobeAPIGroup/AudibleGlobeAPIWorkerRole/Endpoint1" />
          </toPorts>
        </lBChannel>
      </channels>
      <maps>
        <map name="MapAudibleGlobeAPIWorkerRole:Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" kind="Identity">
          <setting>
            <aCSMoniker name="/AudibleGlobeAPI/AudibleGlobeAPIGroup/AudibleGlobeAPIWorkerRole/Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" />
          </setting>
        </map>
        <map name="MapAudibleGlobeAPIWorkerRoleInstances" kind="Identity">
          <setting>
            <sCSPolicyIDMoniker name="/AudibleGlobeAPI/AudibleGlobeAPIGroup/AudibleGlobeAPIWorkerRoleInstances" />
          </setting>
        </map>
      </maps>
      <components>
        <groupHascomponents>
          <role name="AudibleGlobeAPIWorkerRole" generation="1" functional="0" release="0" software="C:\dev\base\Personal\AudibleGlobeAPI\AudibleGlobeAPI\csx\Debug\roles\AudibleGlobeAPIWorkerRole" entryPoint="base\x64\WaHostBootstrapper.exe" parameters="base\x64\WaWorkerHost.exe " memIndex="-1" hostingEnvironment="consoleroleadmin" hostingEnvironmentVersion="2">
            <componentports>
              <inPort name="Endpoint1" protocol="http" portRanges="80" />
            </componentports>
            <settings>
              <aCS name="Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" defaultValue="" />
              <aCS name="__ModelData" defaultValue="&lt;m role=&quot;AudibleGlobeAPIWorkerRole&quot; xmlns=&quot;urn:azure:m:v1&quot;&gt;&lt;r name=&quot;AudibleGlobeAPIWorkerRole&quot;&gt;&lt;e name=&quot;Endpoint1&quot; /&gt;&lt;/r&gt;&lt;/m&gt;" />
            </settings>
            <resourcereferences>
              <resourceReference name="DiagnosticStore" defaultAmount="[4096,4096,4096]" defaultSticky="true" kind="Directory" />
              <resourceReference name="EventStore" defaultAmount="[1000,1000,1000]" defaultSticky="false" kind="LogStore" />
            </resourcereferences>
          </role>
          <sCSPolicy>
            <sCSPolicyIDMoniker name="/AudibleGlobeAPI/AudibleGlobeAPIGroup/AudibleGlobeAPIWorkerRoleInstances" />
            <sCSPolicyUpdateDomainMoniker name="/AudibleGlobeAPI/AudibleGlobeAPIGroup/AudibleGlobeAPIWorkerRoleUpgradeDomains" />
            <sCSPolicyFaultDomainMoniker name="/AudibleGlobeAPI/AudibleGlobeAPIGroup/AudibleGlobeAPIWorkerRoleFaultDomains" />
          </sCSPolicy>
        </groupHascomponents>
      </components>
      <sCSPolicy>
        <sCSPolicyUpdateDomain name="AudibleGlobeAPIWorkerRoleUpgradeDomains" defaultPolicy="[5,5,5]" />
        <sCSPolicyFaultDomain name="AudibleGlobeAPIWorkerRoleFaultDomains" defaultPolicy="[2,2,2]" />
        <sCSPolicyID name="AudibleGlobeAPIWorkerRoleInstances" defaultPolicy="[1,1,1]" />
      </sCSPolicy>
    </group>
  </groups>
  <implements>
    <implementation Id="44811237-c595-49c6-8c3e-175581ddc2de" ref="Microsoft.RedDog.Contract\ServiceContract\AudibleGlobeAPIContract@ServiceDefinition">
      <interfacereferences>
        <interfaceReference Id="34623677-d888-4b79-8792-2cd42c0e2b32" ref="Microsoft.RedDog.Contract\Interface\AudibleGlobeAPIWorkerRole:Endpoint1@ServiceDefinition">
          <inPort>
            <inPortMoniker name="/AudibleGlobeAPI/AudibleGlobeAPIGroup/AudibleGlobeAPIWorkerRole:Endpoint1" />
          </inPort>
        </interfaceReference>
      </interfacereferences>
    </implementation>
  </implements>
</serviceModel>