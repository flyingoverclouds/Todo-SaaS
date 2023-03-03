terraform {
  required_version = "= 1.3.7"


  backend "azurerm" {
    resource_group_name  = "TodoSaaS-common-infra"
    storage_account_name = "tdsiacterraform"
    container_name       = "tenantdeploy"
    key                  = "MY_TFSTATE"
  }

  required_providers {
    azurerm = {
      source  = "hashicorp/azurerm"
      version = "= 3.43.0"
    }
  }
}

provider "azurerm" {
  features {}
  subscription_id = var.AzureSubscriptionID
  client_id       = var.AzureClientID
  client_secret   = var.AzureClientSecret
  tenant_id       = var.AzureTenandID
}

