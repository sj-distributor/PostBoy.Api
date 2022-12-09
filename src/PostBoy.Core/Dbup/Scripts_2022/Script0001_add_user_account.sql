create table if not exists role
(
    id varchar(36) not null primary key,
    created_on datetime(3) not null,
    modified_on datetime(3) not null,
    uuid varchar(36) not null,
    name varchar(512) charset utf8 not null,
    constraint idx_name
    unique (name)
    )
    charset=utf8mb4;

create table if not exists role_user
(
    id varchar(36) not null primary key,
    created_on datetime(3) not null,
    modified_on datetime(3) not null,
    uuid varchar(36) not null,
    role_id int not null,
    user_id int not null,
    constraint idx_user_id_role_id
    unique (user_id, role_id)
    )
    charset=utf8mb4;

create table if not exists user_account
(
    id varchar(36) not null primary key,
    created_on datetime(3) not null,
    modified_on datetime(3) not null,
    uuid varchar(36) not null,
    username varchar(512) charset utf8 not null,
    password varchar(64) not null,
    active tinyint(1) default 1 not null,
    constraint idx_username
    unique (username)
    )
    charset=utf8mb4;