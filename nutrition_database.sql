create database nutrition_db

use master
drop database nutrition_web

use nutrition_db

/* create table */

create table accounts (
	email varchar(25) primary key not null,
	password varchar(25) not null
)

create table users (
	user_id int primary key identity(1,1),
	user_name nvarchar(30),
	email varchar(25) foreign key references accounts(email),
	age int,
	gender bit,
	number varchar(15),
	address nvarchar(100),
	height float,
	weight float,
	health_info nvarchar(150),
	eating_habits nvarchar(150)
)

create table experts (
	expert_id int primary key identity(1,1),
	expert_name nvarchar(30),
	email varchar(25) foreign key references accounts(email),
	age int,
	gender bit,
	number varchar(15),
	address nvarchar(100),
	expertise nvarchar(150)
)

create table nutrition_plans (
	plan_id int primary key identity(1,1),
	user_id int foreign key references users(user_id) not null,
	expert_id int foreign key references experts(expert_id) not null,
	create_date datetime,
	start_date datetime,
	end_date datetime,
	plan_name nvarchar(100)
)

create table nutrition_plan_details (
	plan_detail_id int primary key identity(1,1),
	plan_id int foreign key references nutrition_plans(plan_id) not null,
	calories int,
	start_date datetime,
	end_date datetime,
	plans nvarchar(150),
	food_suggestion nvarchar(150)
)

create table consultations (
	consultation_id int primary key identity(1,1),
	user_id int foreign key references users(user_id) not null,
	expert_id int foreign key references experts(expert_id) not null,
	create_date datetime,
	notes nvarchar(150),
	status varchar(20) not null default 'Pending'
)

create table health_trackings(
	tracking_id int primary key identity(1,1),
	user_id int foreign key references users(user_id) not null,
	create_date datetime not null,
	height float,
	weight float,
	notes nvarchar (150)
)

/* procedure insert */
/* account */
create proc insert_account
	@email nvarchar(25),
	@password nvarchar(25)
as
begin
	insert into accounts(email, password)
	values (@email, @password)
end
/* user */
create proc insert_user
	@user_name nvarchar(30),
	@email varchar(25),
	@age int,
	@gender bit,
	@number varchar(15),
	@address nvarchar(100),
	@height float,
	@weight float,
	@health_info nvarchar(150),
	@eating_habits nvarchar(150)
as
begin 
	if not exists (select 1 from users where number = @number)
	begin
		insert into users (user_name, email, age, gender, number, address, height, weight, health_info, eating_habits)
		values (@user_name, @email, @age, @gender, @number, @address, @height, @weight, @health_info, @eating_habits)
	end
	else
	begin
		print 'This phone number already exists'
	end
end
/* expert */
create proc insert_expert
	@expert_name nvarchar(30),
	@email varchar(25),
	@age int,
	@gender bit,
	@number varchar(15),
	@address nvarchar(100),
	@expertise nvarchar(150)
as
begin
	if not exists (select 1 from experts where number = @number)
	begin
		insert into experts(expert_name, email, age, gender, number, address, expertise)
		values (@expert_name, @email, @age, @gender, @number, @address, @expertise)
	end
	else
	begin
		print 'This phone number already exists'
	end
end

/* nutrition plan */
create proc insert_nutrition_plan
	@user_id int,
	@expert_id int,
	@create_date datetime,
	@start_date datetime,
	@end_date datetime,
	@plan_name nvarchar(100)
as
begin
	insert into nutrition_plans(user_id, expert_id, create_date, start_date, end_date, plan_name)
	values (@user_id, @expert_id, @create_date, @start_date, @end_date, @plan_name)
end

create proc insert_nutrition_plan_detail
	@plan_id int,
	@calories int,
	@start_date datetime,
	@end_date datetime,
	@plans nvarchar(150),
	@food_suggestion nvarchar(150)
as
begin
	insert into nutrition_plan_details(plan_id, calories, start_date, end_date, plans, food_suggestion)
	values (@plan_id, @calories, @start_date, @end_date, @plans, @food_suggestion)
end

/* consultations */
create proc insert_consultation
	@user_id int,
	@expert_id int,
	@create_date datetime,
	@notes nvarchar(150),
	@status varchar(20)= 'Pending'
as
begin
	insert into consultations(user_id, expert_id, create_date, notes, status)
	values (@user_id, @expert_id, @create_date, @notes, @status)
end

/* health tracking */
create proc insert_health_tracking
	@user_id int,
	@create_date datetime,
	@height float,
	@weight float,
	@notes nvarchar(150)
as
begin
	insert into health_trackings(user_id, create_date, height, weight, notes)
	values (@user_id, @create_date, @height, @weight, @notes)
end

/* insert data */
/* account */
exec insert_account 'ngquanganh11a1@gmail.com', '1234'
exec insert_account 'toibingu@gmail.com', '1234'
exec insert_account 'toibingao@gmail.com', '1234'

/* user */
exec insert_user N'Quang Ánh', 'ngquanganh11a1@gmail.com', 20, 1, '0936094129', N'Số 245 Khâm Thiên', 168, 73, N'bình thường', N'ăn uống bất thường'
exec insert_user N'Lương Phượng', 'toibingu@gmail.com', 20, 0, '0987654321', N'Số 96 Định Công', 160, 50, N'bình thường', N'ăn uống bất thường'
exec insert_user N'Minh Châu', 'toibingao@gmail.com', 21, 0, '0123456789', N'Số 96 Định Công', 165, 55, N'bình thường', N'ăn uống bất thường'

/* expert */
exec insert_expert N'NQ Ánh', 'ngquanganh11a1@gmail.com', 20, 1, '0936094129', N'Số 245 Khâm Thiên', N'10 năm kinh nghiệm ngủ ko yên giấc'
exec insert_expert N'Lương Phượng', 'toibingao@gmail.com', 20, 0, '0987654321', N'Số 96 Định Công', N'10 năm kinh nghiệm ngủ ko yên giấc'
exec insert_expert N'Minh Châu', 'toibingu@gmail.com', 21, 0, '0123456789', N'Số 96 Định Công', N'10 năm kinh nghiệm ngủ ko yên giấc'

/* nutrition plan */
exec insert_nutrition_plan 1,2, '2024-10-1', '2024-10-1', '2024-10-15',N'Giảm cân'
exec insert_nutrition_plan 2,3, '2024-10-1', '2024-10-1', '2024-10-15',N'Tăng cân'
exec insert_nutrition_plan 3,1, '2024-10-1', '2024-10-1', '2024-10-15',N'Giảm cân'

/* plan detail */
exec insert_nutrition_plan_detail 1, 2000, '2024-10-1', '2024-10-7', N'Giảm cân từ từ', N'Ăn gì chả được'
exec insert_nutrition_plan_detail 1, 1500, '2024-10-7', '2024-10-14', N'Giảm cân nhanh', N'Ăn gì chả được'

exec insert_nutrition_plan_detail 2, 2000, '2024-10-1', '2024-10-6', N'Tăng cân từ từ', N'Ăn gì chả được'
exec insert_nutrition_plan_detail 2, 2000, '2024-10-7', '2024-10-15', N'Tăng cân nhanh', N'Ăn gì chả được'

exec insert_nutrition_plan_detail 3, 2000, '2024-10-1', '2024-10-10', N'Giảm cân từ từ', N'Ăn gì chả được'
exec insert_nutrition_plan_detail 3, 2000, '2024-10-11', '2024-10-15', N'Giảm cân từ từ', N'Ăn gì chả được'

/* consultation */
exec insert_consultation 1,3, '2024-1-5', N'Check var giảm cân', 'Confirmed'
exec insert_consultation 1,3, '2024-1-10', N'Check var giảm cân'

exec insert_consultation 2,1, '2024-1-5', N'Check var giảm cân'
exec insert_consultation 2,1, '2024-1-10', N'Check var giảm cân', 'Cancelled'

exec insert_consultation 3,2, '2024-1-5', N'Check var giảm cân', 'Confirmed'
exec insert_consultation 3,2, '2024-1-10', N'Check var giảm cân', 'Cancelled'

/* health tracking */
exec insert_health_tracking 1, '2024-10-1', 168,60,N'Chán note rồi'
exec insert_health_tracking 2, '2024-10-1', 168,60,N'Chán note rồi'
exec insert_health_tracking 3, '2024-10-1', 168,60,N'Chán note rồi'

/* select */
select * from accounts
select * from users
select * from experts
select * from nutrition_plans
select * from nutrition_plan_details
select * from consultations
select * from health_trackings

