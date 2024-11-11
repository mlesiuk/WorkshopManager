variable "resource_group_name" {
  type = string
}

variable "location" {
  type = string
}

variable "environment" {
  type        = string
  description = "Environment"

  validation {
    condition     = contains(["dev", "test", "prod"], var.environment)
    error_message = "Allowed values: 'dev', 'test', 'prod'."
  }
}

variable "tenant_id" {
  type    = string
  default = null
}

variable "subscription_id" {
  type    = string
  default = null
}

variable "tags" {
  type = map(string)
}

variable "modules_repository" {
  type    = string
  default = null
}
