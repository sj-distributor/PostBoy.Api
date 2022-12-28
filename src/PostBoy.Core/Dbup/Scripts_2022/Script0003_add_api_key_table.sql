create table if not exists user_account_api_key
(
    `id` char(36) NOT NULL,
    `user_account_id` char(36) NOT NULL,
    `api_key` varchar(128) NOT NULL,
    `description` varchar(256) NULL,
    PRIMARY KEY (`id`)
)charset=utf8mb4;