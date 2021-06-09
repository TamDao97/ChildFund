// JavaScript Document

//function validate
function is_username(obj)
{
	var Re_username = new RegExp(/^([a-z|A-Z|_|\.|0-9][a-z|A-Z|_|\.|0-9]+)$/);
	return Re_username.test(obj);
	//username ki tự đầu phải là kí tự chữ. k thể là số. các kí tự sau lặp có thẻ là chứ _ .  or sô
}

function is_password(obj)
{
	Re_password = new RegExp(/^([a-z|A-Z|0-9])([a-z|A-Z|0-9][a-z|A-Z|0-9][a-z|A-Z|0-9][a-z|A-Z|0-9])([a-z|A-Z|0-9]+)$/);
	return Re_password.test(obj);
	//password : 6 kí tự
}

function is_phone(obj)
{
	var Re_phone = new RegExp(/^(\d{6,11})$/);
	return Re_phone.test(obj);
	//phone : 6-11 số
}

function is_Email(obj)
{
	var pattern = new RegExp(/^(("[\w-+\s]+")|([\w-+]+(?:\.[\w-+]+)*)|("[\w-+\s]+")([\w-+]+(?:\.[\w-+]+)*))(@((?:[\w-+]+\.)*\w[\w-+]{0,66})\.([a-z]{2,6}(?:\.[a-z]{2})?)$)|(@\[?((25[0-5]\.|2[0-4][0-9]\.|1[0-9]{2}\.|[0-9]{1,2}\.))((25[0-5]|2[0-4][0-9]|1[0-9]{2}|[0-9]{1,2})\.){2}(25[0-5]|2[0-4][0-9]|1[0-9]{2}|[0-9]{1,2})\]?$)/i);
	return pattern.test(obj);	
}

function is_Date(obj)
{
	var Re_date = new RegExp(/^(\d{4})([\.|\/|-])(\d{1,2})([\.|\/|-])(\d{1,2})$/);
	return Re_date.test(obj);
	//bắt định dạng : yyyy-mm-dd hoặc yyyy-mm-dd hh:pp
}

function is_number(obj)
{
	var Re_phone = new RegExp(/^\d+$/);
	return Re_phone.test(obj);
	//chi co the la so
}

/*-------------------validate page-----------------------*/
//validate mod=contact
function mod_contact() {
	//var flag test
	flag1 = 0;
	flag2 = 0;
	flag3 = 0; 
	
	//validate user
	if($('#username').val() == "")
	{
		$('#error_username').html('Tên không được để trống !');	
		flag1 = 0;
	}
	else
	{
		$('#error_username').html('');	
		flag1 = 1;
	}
	
	if($('#create_date').val() == "")
	{
		$('#error_create_date').html('Ngày gửi không được để trống !');	
		flag2 = 0;
	}
	else if(is_Date($('#create_date').val())== false)
	{
		$('#error_create_date').html('Ngày gửi nhập sai quy tắc (yyyy-mm-dd)!');	
		flag2 = 0;
	}
	else
	{
		$('#error_create_date').html('');	
		flag2 = 1;
	}
	
	if($('#email').val() == "")
	{
		$('#error_email').html('Email không được để trống !');	
		flag3 = 0;
	}
	else if(is_Email($('#email').val())== false)
	{
		$('#error_email').html('Email nhập sai quy tắc (ShopHoaOnline@XXX.XX)!');	
		flag3 = 0;
	}
	else
	{
		$('#error_email').html('');	
		flag3 = 1;
	}
	
	
	
	if(flag1 == 0 || flag2 == 0 || flag3 == 0 )
		return false;
	else
		return true;

}
/*-------------------validate page user---------------------------*/
//validate mod=user
function mod_user() {
	//var flag test
	flag1 = 1;
	flag2 = 1;
	flag3 = 0;
	flag4 = 1;
	flag5 = 1;
	flag6 = 0;
	flag7 = 0;
	flag8 = 0;
	
	if($('#new_password').val() != "") 
	{
	
		if($('#password').val() == "")
		{
			$('#error_password').html('Mật khẩu không được để trống !');	
			flag1 = 0;
		}
		else
		{
			$('#error_password').html('');	
			flag1 = 1;
		}
		
		if(is_password($('#new_password').val()) == false)
		{
			$('#error_new_password').html('Mật khẩu thay đổi ít nhất 6 kí tự !');	
			flag2 = 0;
		}
		else
		{
			$('#error_new_password').html('');	
			flag2 = 1;
		}
		
		if($('#new_password').val() != $('#re_password').val())
		{
			$('#error_re_password').html('Mật khẩu nhập lại không đúng !');	
			flag4 = 0;
		}
		else
		{
			$('#error_re_password').html('');	
			flag4 = 1;
		}
		
	
	}
	
	if($('#email').val() == "")
	{
		$('#error_email').html('Email không được để trống !');	
		flag3 = 0;
	}
	else if(is_Email($('#email').val())== false)
	{
		$('#error_email').html('Email nhập sai quy tắc (ShopHoaOnline@XXX.XX)!');	
		flag3 = 0;
	}
	else
	{
		$('#error_email').html('');	
		flag3 = 1;
	}
	
	if($('#re_email').val() != "") {
		if($('#email').val() != $('#re_email').val())
		{
			$('#error_re_email').html('Email không giống nhau !');	
			flag5 = 0;
		}	
		else
		{
			$('#error_re_email').html('');	
			flag5 = 1;
		}
	}
	//name
	if($('#name').val() =="")
	{
		$('#error_name').html('Tên không được để trống !');	
		flag6 = 0;
	}	
	else
	{
		$('#error_name').html('');	
		flag6 = 1;
	}
	
	//phone
	if($('#phone').val() =="")
	{
		$('#error_phone').html('Số điện thoại không được để trống !');	
		flag7 = 0;
	}	
	else
	{
		$('#error_phone').html('');	
		flag7 = 1;
	}
	
	//address
	if($('#address').val() =="")
	{
		$('#error_address').html('Số điện thoại không được để trống !');	
		flag8 = 0;
	}	
	else
	{
		$('#error_address').html('');	
		flag8= 1;
	}
	
	
	
	
	if(flag1 == 0 || flag2 == 0 || flag3 == 0 || flag4 == 0 || flag5 == 0 || flag6 == 0 || flag7 == 0 || flag8 == 0 )
		return false;
	else
		return true;

}
/*-------------------validate page register-----------------------*/
//validate mod=register
function mod_register() {
	//var flag test
	flag1 = 0;
	flag2 = 0;
	flag3 = 0;
	flag4 = 0;
	flag5 = 0;
	flag6 = 0;
	flag7 = 0;
	flag8 = 0;
	
	//validate username
	if($('#username').val() == "")
	{
		$('#error_username').html('Tên không được để trống !');	
		flag1 = 0;
	}
	else if(is_username($('#username').val())== false)
	{
		$('#error_username').html('Tên đăng nhập nhập sai quy tắc!');	
		flag1 = 0;
	}
	else
	{
		$('#error_username').html('');	
		flag1 = 1;
	}
	
	//validate password
	if($('#password').val() == "")
	{
		$('#error_password').html('Mật khẩu không được để trống !');	
		flag2 = 0;
	}
	else if(is_password($('#password').val())== false)
	{
		$('#error_password').html('Mật khẩu nhập sai quy tắc(tối thiểu 6 kí tự)!');	
		flag2 = 0;
	}
	else
	{
		$('#error_password').html('');	
		flag2 = 1;
	}
	
	//validate re-password
	if($('#password').val() != $('#re_password').val())
	{
		$('#error_re_password').html('Mật khẩu nhập lại không giống nhau!');	
		flag3 = 0;
	}
	else
	{
		$('#error_re_password').html('');	
		flag3 = 1;
	}
	
	//validate email
	if($('#email').val() == "")
	{
		$('#error_email').html('Email không được để trống !');	
		flag4 = 0;
	}
	else if(is_Email($('#email').val())== false)
	{
		$('#error_email').html('Email nhập sai quy tắc (ShopHoaOnline@XXX.XX)!');	
		flag4 = 0;
	}
	else
	{
		$('#error_email').html('');	
		flag4 = 1;
	}
	
	//validate re-email
	if($('#email').val() != $('#re_email').val())
	{
		$('#error_re_email').html('Email nhập lại không giống nhau!');	
		flag5 = 0;
	}
	else
	{
		$('#error_re_email').html('');	
		flag5 = 1;
	}
	
	//validate phone
	flag6 = 1;
	if($('#phone').val() != "") {
		if(is_phone($('#phone').val()) == false)
		{
			$('#error_phone').html('Số điện thoại chỉ là số(6-11 số)!');	
			flag6 = 0;
		}
		else
		{
			$('#error_phone').html('');	
			flag6 = 1;
		}
	}
	
	if(flag1 == 0 || flag2 == 0 || flag3 == 0 || flag4 == 0 || flag5 == 0 || flag6 == 0 )
		return false;
	else
		return true;

}

/*-------------------validate page edit password-----------------------*/
//validate mod=edit_password
function mod_edit_password() {
	//var flag test
	flag1 = 0;
	flag2 = 0;
	
	//validate password
	if($('#password').val() == "")
	{
		$('#error_password').html('Mật khẩu không được để trống !');	
		flag1 = 0;
	}
	else if(is_password($('#password').val())== false)
	{
		$('#error_password').html('Mật khẩu nhập sai quy tắc(tối thiểu 6 kí tự)!');	
		flag1 = 0;
	}
	else
	{
		$('#error_password').html('');	
		flag1 = 1;
	}
	
	//validate re-password
	if($('#password').val() != $('#re_password').val())
	{
		$('#error_re_password').html('Mật khẩu nhập lại không giống nhau!');	
		flag2 = 0;
	}
	else
	{
		$('#error_re_password').html('');	
		flag2 = 1;
	}
	
	if(flag1 == 0 || flag2 == 0)
		return false;
	else
		return true;

}
