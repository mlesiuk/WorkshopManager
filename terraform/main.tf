terraform {
  required_version = "~> 1.9.8"

  required_providers {
    azurerm = {
      source  = "hashicorp/azurerm"
      version = "~> 4.8.0"
    }
  }

  backend "http" {
    encrypt = true
  }
}

provider "azurerm" {
  features {
  }
  
  subscription_id            = var.subscription_id
  storage_use_azuread        = true
}

data "azurerm_client_config" "current" {}

locals {
  tenant_id       = var.tenant_id
  subscription_id = var.subscription_id

  key_vault_account_name = "kv-wm-${var.environment}"
}

resource "azurerm_resource_group" "resource_group" {
  name     = var.resource_group_name
  location = var.location
  tags     = var.tags
}

module "key_vault" {
  source = "gitlab.com/cicd8371448/keyvault/azure"

  name                        = local.key_vault_account_name
  location                    = azurerm_resource_group.resource_group.location
  resource_group_name         = azurerm_resource_group.resource_group.name
  enable_rbac_authorization   = true
  enabled_for_disk_encryption = true
  tenant_id                   = data.azurerm_client_config.current.tenant_id
  soft_delete_retention_days  = 7
  purge_protection_enabled    = true
  sku_name                    = "standard"
  tags                        = var.tags

  network_acls = {
    default_action = "Deny"
    bypass         = "AzureServices"
    ip_rules       = []
  }
  access_policy = null
}
