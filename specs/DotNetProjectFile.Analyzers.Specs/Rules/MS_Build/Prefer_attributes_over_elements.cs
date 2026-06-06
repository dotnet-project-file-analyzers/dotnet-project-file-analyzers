namespace Specs.Rules.MS_Build.Prefer_attributes_over_elements;

public class Reports
{
    [Test]
    public void elements_that_can_be_attributes() => new PreferAttributesOverElements()
        .ForInlineCsproj(""""
         <Project Sdk="Microsoft.NET.Sdk">

           <PropertyGroup>
             <TargetFramework>net10.0</TargetFramework>
           </PropertyGroup>

           <ItemGroup Label="Noncompliant">
             <PackageReference Include="Qowaiv">
        	   <Version>8.0.0</Version>
        	   <PrivateAssets>
        	   none
        	   </PrivateAssets>
        	 </PackageReference>
           </ItemGroup>
           
           <ItemGroup Label="Compliant conditional">
        	 <PackageReference Update="coverlet.collector">
        	   <Version Condition="'$(Configuration)'=='Release'">10.0.0</Version>
        	   <Version Condition="'$(Configuration)'!='Release'">10.0.1</Version>
        	 </PackageReference>
           </ItemGroup>

           <ItemGroup Label="Compliant multi-line">
              <CustomItem Include="reference">
        	    <Documentation>
        	      Lorem ipsum dolor sit amet, consectetuer adipiscing elit. Aenean
        		  commodo ligula eget dolor. Aenean massa. Cum sociis natoque
        		  penatibus et magnis dis parturient montes, nascetur ridiculus mus.
        		</Documentation>
        	  </CustomItem>
           </ItemGroup>

         </Project>
        """")
       .HasIssues(
            Issue.WRN("Proj0052", "Consider <Version> as an attribute"/*..*/).WithSpan(08, 04, 08, 28),
            Issue.WRN("Proj0052", "Consider <PrivateAssets> as an attribute").WithSpan(09, 04, 11, 20));
}
