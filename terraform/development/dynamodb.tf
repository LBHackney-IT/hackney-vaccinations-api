resource "aws_dynamodb_table" "notifications_table" {
    name                  = "Notifications"
    billing_mode          = "PROVISIONED"
    read_capacity         = 10
    write_capacity        = 10
    hash_key              = "id"

    attribute {
        name              = "id"
        type              = "S"
    }
    attribute {
        name              = "target_id"
        type              = "S"
    }
    tags = {
        Name              = "lbh-notifications-api-${var.environment_name}"
        Environment       = var.environment_name
        terraform-managed = true
        project_name      = var.project_name
    }

     global_secondary_index {
        name               = "target_id_dx"
        hash_key           = "target_id"
        write_capacity     = 10
        read_capacity      = 10
        projection_type    = "ALL"
    }
    point_in_time_recovery {
        enabled           = true
    }
}
