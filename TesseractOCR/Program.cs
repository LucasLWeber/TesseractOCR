using Amazon;
using Amazon.Runtime;
using Amazon.SQS;
using TesseractOCR.Application.Interfaces;
using TesseractOCR.Application.Services;
using TesseractOCR.Infrastructure.Config;
using TesseractOCR.Infrastructure.Services;
using TesseractOCR.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Application and UseCase
builder.Services.AddScoped<ITesseractOcrService, TesseractOcrService>();
builder.Services.AddScoped<GetDocumentsFromMongoUseCase>();

// Load AWS credentials from appsettings.json
var awsAccessKey = builder.Configuration["AWS:Credentials:AccessKey"];
var awsSecretKey = builder.Configuration["AWS:Credentials:SecretKey"];
var region = RegionEndpoint.GetBySystemName(builder.Configuration["AWS:Region"]);

// Create AWS credentials object
var awsCredentials = new BasicAWSCredentials(awsAccessKey, awsSecretKey);

// Register SQS client with manual credentials
builder.Services.AddSingleton<IAmazonSQS>(sp =>
{
    return new AmazonSQSClient(awsCredentials, region);
});

builder.Services.AddScoped<IQueuePublisher, SqsQueueSender>();
builder.Services.AddScoped<IQueueConsumer, SqsQueueConsumer>();
builder.Services.AddHostedService<SqsQueueConsumerWorker>();

// Bind options for SQS queue settings
builder.Services.Configure<AwsSqsOptions>(builder.Configuration.GetSection("AwsSqs"));

builder.Services.AddScoped<ITesseractOcrDocumentResult, MongoTesseractOcrDocumentResult>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
