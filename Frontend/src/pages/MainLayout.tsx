import { Outlet } from "react-router-dom";
import { useNavigate } from "react-router-dom";

export default function MainLayout() {
  const navigate = useNavigate();

  return (
    <div className="min-h-screen bg-gray-100">
      {/* Navbar */}
      <nav className="bg-white border-b border-gray-200 shadow-sm">
        <div className="max-w-6xl mx-auto px-6 py-4 flex items-center justify-between">
          {/* Logo / Brand */}
          <button
            onClick={() => navigate("/")}
            className="text-2xl font-semibold text-gray-800 hover:text-blue-600 transition"
          >
            Store
          </button>

          {/* Nav Links */}
          <div className="flex items-center gap-4">
            <button
              onClick={() => navigate("/")}
              className="px-4 py-2 rounded-md text-gray-700 hover:bg-gray-100 hover:text-blue-600 transition"
            >
              Home
            </button>

            <button
              onClick={() => navigate("/login")}
              className="px-5 py-2 rounded-md bg-blue-600 text-white font-medium hover:bg-blue-700 transition shadow-sm"
            >
              Login
            </button>
          </div>
        </div>
      </nav>

      {/* Page Content */}
      <main className="max-w-6xl mx-auto px-6 py-8">
        <Outlet />
      </main>
    </div>
  );
}
