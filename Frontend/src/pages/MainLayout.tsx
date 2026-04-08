import { Outlet } from "react-router-dom";
import { useNavigate } from "react-router-dom";

export default function MainLayout() {
  const navigate = useNavigate();
  return (
    <div className="flex flex-col h-screen overflow-hidden bg-white">
      <div className="flex justify-between items-center mb-8">
        <button
          onClick={() => navigate("/")}
          className="text-3xl font-bold hover:bg-gray-100 transition"
        >
          Store
        </button>

        <button
          onClick={() => navigate("/login")}
          className="bg-black text-white px-4 py-2 rounded-xl hover:bg-gray-800 transition"
        >
          Login
        </button>
      </div>
      <div className="flex flex-1 overflow-hidden bg-gray-50">
        <main className="flex-1 overflow-y-auto bg-gray-100 p-4">
          <Outlet />
        </main>
      </div>
    </div>
  );
}
