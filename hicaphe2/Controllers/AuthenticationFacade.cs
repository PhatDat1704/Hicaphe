using hicaphe2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace hicaphe2.Controllers
{
    // Lớp Facade cho việc đăng nhập và đăng ký
    public class AuthenticationFacade
    {
        private readonly HiCaPheEntities2 _database;

        public AuthenticationFacade(HiCaPheEntities2 database)
        {
            _database = database;
        }

        // Phương thức đăng nhập
        public bool Login(string email, string password)
        {
            // Thực hiện logic đăng nhập và kiểm tra trong CSDL
            var khach = _database.TAIKHOANKHACHHANGs.FirstOrDefault(k => k.Email == email && k.Matkhau == password);
            return khach != null;
        }

        // Phương thức đăng ký
        public bool Register(string hoTen, string email, string matKhau, string sdt)
        {
            // Thực hiện logic đăng ký và kiểm tra trong CSDL
            var existingUser = _database.TAIKHOANKHACHHANGs.FirstOrDefault(k => k.Email == email);
            if (existingUser != null)
                return false; // Trả về false nếu email đã tồn tại trong CSDL

            // Nếu email chưa tồn tại, thực hiện đăng ký
            var newUser = new TAIKHOANKHACHHANG
            {
                HoTenKH = hoTen,
                Email = email,
                Matkhau = matKhau,
                SDT = sdt
            };

            _database.TAIKHOANKHACHHANGs.Add(newUser);
            _database.SaveChanges();
            return true; // Trả về true nếu đăng ký thành công
        }

    }
}