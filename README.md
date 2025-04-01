## eCommerceDs
ASP.NET Core Web API eCommerceDs

![eCommerceDs](img/1.png)
![eCommerceDs](img/2.png)
![eCommerceDs](img/3.png)


## Program
``` 
builder.Services.AddDbContext<AlmacenContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("Connection"))
);
``` 

## appsetting.Development.json
``` 
{
  "ConnectionStrings": {
        "Connection": "Server=*;Database=eCommerceDs;Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=True"
}
``` 