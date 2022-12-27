create table if not exists user_account_api_key
(
    `id` char(36) NOT NULL,
    `user_account_id` varchar(128) NOT NULL,
    `api_key` varchar(128) NOT NULL,
    PRIMARY KEY (`id`)
)charset=utf8mb4;