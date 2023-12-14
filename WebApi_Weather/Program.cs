var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/apis", async () =>
{
    using var client = new HttpClient();
    var apis = await client.GetFromJsonAsync<ApiModel>("https://api.publicapis.org/entries");
    return apis;
})
.WithName("GetApis")
.WithOpenApi();;

app.Run();

internal record ApiModel
{
    public required int Count {  get; set; }
    public required List<ApiEntry> Entries { get; set; }
}

internal record ApiEntry
{ 
    public required string API {  get; set; }
    public required string Description { get; set; }
    public required string Link { get; set; }
}