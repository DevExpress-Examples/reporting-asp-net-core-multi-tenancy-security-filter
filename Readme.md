<!-- default badges list -->
![](https://img.shields.io/endpoint?url=https://codecentral.devexpress.com/api/v1/VersionRange/358396707/24.2.1%2B)
[![](https://img.shields.io/badge/Open_in_DevExpress_Support_Center-FF7200?style=flat-square&logo=DevExpress&logoColor=white)](https://supportcenter.devexpress.com/ticket/details/T990777)
[![](https://img.shields.io/badge/📖_How_to_use_DevExpress_Examples-e9f6fc?style=flat-square)](https://docs.devexpress.com/GeneralInformation/403183)
[![](https://img.shields.io/badge/💬_Leave_Feedback-feecdd?style=flat-square)](#does-this-example-address-your-development-requirementsobjectives)
<!-- default badges end -->
# Reporting for ASP.NET Core - Row-Level Filtering in an Application with SqlDataSource (Multi-Tenancy Support)

This example demonstrates how to restrict access at the row level to the source data based on the user who is logged into the system. Create and register a service that implements the [DevExpress.DataAccess.Web.ISelectQueryFilterService](https://docs.devexpress.com/CoreLibraries/DevExpress.DataAccess.Web.ISelectQueryFilterService) interface. The [ISelectQueryFilterService.CustomizeFilterExpression](https://docs.devexpress.com/CoreLibraries/DevExpress.DataAccess.Web.ISelectQueryFilterService.CustomizeFilterExpression(DevExpress.DataAccess.Sql.SelectQuery-DevExpress.Data.Filtering.CriteriaOperator)) method applies a conditional clause to the query passed to the method as a parameter.  The **Document Viewer**, **Report Designer's Preview**, and **Query Builder** call the `ISelectQueryFilterService` service before the [SqlDataSource](https://docs.devexpress.com/CoreLibraries/DevExpress.DataAccess.Sql.SqlDataSource) executes a SELECT query.

## Implementation details

### Authentication

For ease of demonstration, this example uses a simulated user login (without actual verification) that allows your code to use this user's identity.

### User ID Retrieval

A custom `UserService` service processes the [HttpContext](https://docs.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.http.httpcontext) and retrieves the user ID.


### Security Filter

The `SelectQueryFilterService` service implements the [ISelectQueryFilterService](https://docs.devexpress.com/CoreLibraries/DevExpress.DataAccess.Web.ISelectQueryFilterService) interface. The service calls the `UserService` service to get the ID of the user who is logged into the application.

The service's **CustomizeFilterExpression** method determines whether the query contains the specified tables, and adds conditional clauses that retrieve data rows where the `StudentID` column value matches the current User ID. 

Note that the `ISelectQueryFilterService` does not allow you to modify the query passed to the `CustomizeFilterExpression` method. The method returns the [CriteriaOperator](https://docs.devexpress.com/CoreLibraries/DevExpress.Data.Filtering.CriteriaOperator) that forms the WHERE clause for the original SELECT query.

### Connection String

The `QueryFilterServiceApp` connection string is stored in the secret storage, as Microsoft recommends. Review the following article for more information: [Protect secrets in development](https://docs.microsoft.com/en-us/aspnet/core/security/app-secrets). The content of the `secrets.json` file is:

```
{
  "ConnectionStrings:QueryFilterServiceApp": "XpoProvider=MSSqlServer;Server=(local);Database=QueryFilterServiceApp;MultipleActiveResultSets=true;Integrated Security=True"
}
```
The `RemoveXpoProviderKey` method converts the DevExpress XPO connection string to a connection string that Entity Framework can use.

## Files to Review

- [UserService.cs](QueryFilterServiceApp/Services/UserService.cs)
- [SelectQueryFilterService.cs](QueryFilterServiceApp/Services/SelectQueryFilterService.cs)
- [Startup.cs](QueryFilterServiceApp/Startup.cs)
<!-- feedback -->
## Does this example address your development requirements/objectives?

[<img src="https://www.devexpress.com/support/examples/i/yes-button.svg"/>](https://www.devexpress.com/support/examples/survey.xml?utm_source=github&utm_campaign=reporting-asp-net-core-multi-tenancy-security-filter&~~~was_helpful=yes) [<img src="https://www.devexpress.com/support/examples/i/no-button.svg"/>](https://www.devexpress.com/support/examples/survey.xml?utm_source=github&utm_campaign=reporting-asp-net-core-multi-tenancy-security-filter&~~~was_helpful=no)

(you will be redirected to DevExpress.com to submit your response)
<!-- feedback end -->
