Prerequisite:
	Run the sql at - C:\VSTS\PFEng\Main\Platform\Common\MDRX.DataAccessStandard\Test\Prerequisite_db_scripts - to generate sample database CDALDB
Step 1
To generate artifacts under DemoAppConfig run the generator code (project-MDRX.Dal.Generator.csproj) with following parameters. 
--projectName "DemoApp" --projectNameSpace "DemoApp" --outputFolderPath "C:\VSTS\PFEng\Main\Platform\Common\MDRX.DataAccessStandard\Test\DemoAppConfig\DemoAppArtifacts" --configFolderPath "C:\VSTS\PFEng\Main\Platform\Common\MDRX.DataAccessStandard\Test\DemoAppConfig" --connectionString "Data Source=INMDHIRAN01;Initial Catalog=CDALDB;Integrated Security=True;"

Step 2:
  Copy the data contracts from C:\VSTS\PFEng\Main\Platform\Common\MDRX.DataAccessStandard\Test\DemoAppConfig\DemoAppArtifacts\DataContract to
  C:\VSTS\PFEng\Main\Platform\Common\MDRX.DataAccessStandard\Test\TestPrecompiledModel

  Copy ORMMetadata.xml from folder C:\VSTS\PFEng\Main\Platform\Common\MDRX.DataAccessStandard\Test\DemoAppConfig\DemoAppArtifacts\Metadata
  to C:\VSTS\PFEng\Main\Platform\Common\MDRX.DataAccessStandard\Test\TestPrecompiledModel

  Make sure ORMMetadata.xml is marked as Embeded resource in TestPrecompiledModel project

  Copy precompiled types in folder C:\VSTS\PFEng\Main\Platform\Common\MDRX.DataAccessStandard\Test\DemoAppConfig\DemoAppArtifacts\CompiledDataContractType to folder C:\VSTS\PFEng\Main\Platform\Common\MDRX.DataAccessStandard\Test\TestPrecompiledModel\DemoApp\Server

  				