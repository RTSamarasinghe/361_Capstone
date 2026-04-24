import { useState } from "react";
import api from "../api";
import { useNavigate } from "react-router-dom";
import ErrorNotification from "../components/Error";
export default function Register() {
  const navigate = useNavigate();

  const [username, setUsername] = useState("");
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const [error, setError] = useState<string | null>(null);

  async function handleSubmit(e: React.SubmitEvent) {
      e.preventDefault();
      setError(null); // Clear previous errors
    try {
      const res = await api.post("/auth/register", {
        username,
        email,
        password,
      });
      console.log("User registered:", res.data);
      navigate("/");
    } catch (err: any) {

        if (err.response) {
            const status = err.response.status;

            if (status === 409) {
                setError(err.response.data.message || "Email or username already exists.");
            }

            else if (status === 400) {
                setError(err.response.data.message || "Invalid input. Please check your data and try again.");
            }

            else if (status === 500) {
                setError("Server error. Please try again later.");
            }
            else {
                setError("An unexpected error occurred. Please try again.");
            }

        }
      console.error(err);
    }
    
  }

  return (
    <div className="flex items-center justify-center min-h-screen bg-gray-100">
      <div className="w-full max-w-sm bg-white p-8 rounded-xl shadow-md border border-gray-200">
        <h2 className="text-2xl font-semibold mb-6 text-center text-gray-800">
          Create Account
        </h2>

          <form onSubmit={handleSubmit} className="flex flex-col gap-4">
                  {error && <ErrorNotification message={error} />}
          <input
            type="text"
            placeholder="Username"
            value={username}
            onChange={(e) => setUsername(e.target.value)}
            required
            className="px-4 py-2 rounded-md border border-gray-300 focus:outline-none focus:ring-2 focus:ring-blue-500 focus:border-blue-500"
          />

          <input
            type="email"
            placeholder="Email"
            value={email}
            onChange={(e) => setEmail(e.target.value)}
            required
            className="px-4 py-2 rounded-md border border-gray-300 focus:outline-none focus:ring-2 focus:ring-blue-500 focus:border-blue-500"
          />

          <input
            type="password"
            placeholder="Password"
            value={password}
            onChange={(e) => setPassword(e.target.value)}
            required
            className="px-4 py-2 rounded-md border border-gray-300 focus:outline-none focus:ring-2 focus:ring-blue-500 focus:border-blue-500"
          />

          <button
            type="submit"
            className="mt-2 py-2 rounded-md bg-blue-600 text-white font-medium hover:bg-blue-700 transition"
          >
            Register
          </button>
        </form>

        <div className="mt-6 text-center text-sm text-gray-600">
          Already have an account?{" "}
          <button
            onClick={() => navigate("/login")}
            className="text-blue-600 hover:underline"
          >
            Sign in
          </button>
        </div>
      </div>
    </div>
  );
}
