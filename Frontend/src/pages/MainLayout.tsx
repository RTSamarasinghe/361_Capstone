import { Outlet, useNavigate } from "react-router-dom";
import { mockProducts } from "../temp/mockProducts";

export default function MainLayout() {
  const navigate = useNavigate();

  // TEMP: fake cart
  const cartItems = [
    { ...mockProducts[0], quantity: 1 },
    { ...mockProducts[2], quantity: 2 },
  ];

  const cartCount = cartItems.reduce((sum, item) => sum + item.quantity, 0);

  return (
    <div className="min-h-screen bg-gray-100">
      {/* Navbar */}
      <nav className="bg-white border-b border-gray-200 shadow-sm">
        <div className="max-w-6xl mx-auto px-6 py-4 flex items-center justify-between">
          {/* Logo */}
          <button
            onClick={() => navigate("/")}
            className="text-2xl font-semibold text-gray-800 hover:text-blue-600 transition"
          >
            Store
          </button>

          {/* Nav Links */}
          <div className="flex items-center gap-4">
            {/* Home */}
            <button
              onClick={() => navigate("/")}
              className="px-4 py-2 rounded-md text-gray-700 hover:bg-gray-100 hover:text-blue-600 transition"
            >
              Home
            </button>

            {/* Cart */}
            <button
              onClick={() => navigate("/cart")}
              className="relative px-4 py-2 rounded-md text-gray-700 hover:bg-gray-100 transition"
            >
              🛒
              {cartCount > 0 && (
                <span className="absolute -top-1 -right-1 bg-blue-600 text-white text-xs px-2 py-0.5 rounded-full">
                  {cartCount}
                </span>
              )}
            </button>

            {/* Login */}
            <button
              onClick={() => navigate("/login")}
              className="px-5 py-2 rounded-md bg-blue-600 text-white font-medium hover:bg-blue-700 transition shadow-sm"
            >
              Login
            </button>
          </div>
        </div>
      </nav>

      {/* Content */}
      <main className="max-w-6xl mx-auto px-6 py-8">
        <Outlet />
      </main>
    </div>
  );
}
