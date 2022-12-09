create table if not exists work_wechat_corp
(
    `id` char(36) NOT NULL,
    `corp_id` varchar(128) NOT NULL,
    `corp_name` varchar(56) NOT NULL,
    PRIMARY KEY (`id`)
)charset=utf8mb4;

create table if not exists work_wechat_corp_application
(
    `id` char(36) NOT NULL,
    `work_wechat_corp_id` char(36) NOT NULL,
    `app_id` varchar(128) NOT NULL,
    `name` varchar(56) NOT NULL,
    `secret` varchar(1024) NOT NULL,
    `agent_id` int NOT NULL,
    PRIMARY KEY (`id`)
)charset=utf8mb4;