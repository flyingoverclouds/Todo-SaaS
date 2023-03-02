variable "AzureSubscriptionID" {
  type        = string
  description = "Azure Subscription ID"
}
variable "AzureClientID" {
  type        = string
  description = "Azure Client ID"
}
variable "AzureClientSecret" {
  type        = string
  description = "Azure Client Secret"
}
variable "AzureTenandID" {
  type        = string
  description = "Azure Tenand ID"
}
variable "Location" {
  type        = string
  description = "Azure Location"
}
variable "RgName" {
  type        = string
  description = "Azure Resource Group Name"
}
variable "CodeClient" {
  type        = string
  description = "Client Number"
}
variable "EnvLogsName" {
  type = string
  description = "Azure Logs Analytic Name"
  
}
variable "EnvAcaName" {
  type        = string
  description = "Azure Environnmment Aca Name"
}
variable "CosmosDBAccountName" {
  type = string
  description = "Azure CosmosDB account name"
}
variable "CosmosDataBaseName" {
  type = string
  description = "Azure CosmosDB account name"  
}
variable "ContainerNameCosmasDB" {
  type = string
  description = "Azure CosmosDB Index name"
}
variable "Throughput" {
    type = number
    description = "Cosmos db Data Base throughput"
}