resource "azurerm_resource_group" "rg_env" {
  name     = var.RgName
  location = var.Location
  tags = {
    client = var.CodeClient
  }
}

resource "azurerm_log_analytics_workspace" "log_env" {
  name                = var.EnvLogsName
  location            = azurerm_resource_group.rg_env.location
  resource_group_name = azurerm_resource_group.rg_env.name
  sku                 = "PerGB2018"
  retention_in_days   = 30
}

resource "azurerm_container_app_environment" "aca_env" {
  name                       = var.EnvAcaName
  location                   = azurerm_resource_group.rg_env.location
  resource_group_name        = azurerm_resource_group.rg_env.name
  log_analytics_workspace_id = azurerm_log_analytics_workspace.log_env.id
  tags = {
    client = var.CodeClient
  }

}

resource "azurerm_cosmosdb_account" "cosmosdb_account_aca" {
  name                      = var.CosmosDBAccountName
  location                  = azurerm_resource_group.rg_env.location
  resource_group_name       = azurerm_resource_group.rg_env.name
  offer_type                = "Standard"
  kind                      = "GlobalDocumentDB"
  enable_automatic_failover = false
  geo_location {
    location          = azurerm_resource_group.rg_env.location
    failover_priority = 0
  }
  consistency_policy {
    consistency_level       = "BoundedStaleness"
    max_interval_in_seconds = 300
    max_staleness_prefix    = 100000
  }
  tags = {
    client = var.CodeClient
  }
}

resource "azurerm_cosmosdb_sql_database" "cosmosdb_db_aca" {
  name                = var.CosmosDataBaseName
  resource_group_name = azurerm_resource_group.rg_env.name
  account_name        = azurerm_cosmosdb_account.cosmosdb_account_aca.name
  throughput          = var.Throughput
}

resource "azurerm_cosmosdb_sql_container" "cosmosdb_index01" {
  name                  = var.ContainerNameCosmasDB
  resource_group_name   = azurerm_resource_group.rg_env.name
  account_name          = azurerm_cosmosdb_account.cosmosdb_account_aca.name
  database_name         = azurerm_cosmosdb_sql_database.cosmosdb_db_aca.name
  partition_key_path    = "/definition/id"
  partition_key_version = 1
  throughput            = var.Throughput

  indexing_policy {
    indexing_mode = "consistent"

    included_path {
      path = "/*"
    }

    included_path {
      path = "/included/?"
    }

    excluded_path {
      path = "/excluded/?"
    }
  }
  unique_key {
    paths = ["/definition/idlong", "/definition/idshort"]
  }
}
