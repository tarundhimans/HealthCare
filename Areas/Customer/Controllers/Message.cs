﻿using E_Healthcare.Data;
using Microsoft.AspNetCore.Mvc;

namespace E_Healthcare.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class Message : Controller
    {
        private readonly ApplicationDbContext _context;
        public Message(ApplicationDbContext context)
        {
                _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
