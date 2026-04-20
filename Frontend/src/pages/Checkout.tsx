import { useState } from "react";
import { useNavigate } from "react-router-dom";
import { mockProducts } from "../temp/mockProducts";

export default function Checkout() {
  const navigate = useNavigate();

  // fake cart again (replace later with real cart state)
  const cartItems = [
    { ...mockProducts[0], quantity: 1 },
    { ...mockProducts[2], quantity: 2 },
  ];

  const subtotal = cartItems.reduce(
    (sum, item) => sum + item.price * item.quantity,
    0,
  );

  const [form, setForm] = useState({
    name: "",
    address: "",
    city: "",
    state: "",
    zip: "",
    cardNumber: "",
    expiration: "",
    cvv: "",
  });

  function handleChange(e: React.ChangeEvent<HTMLInputElement>) {
    setForm({ ...form, [e.target.name]: e.target.value });
  }

  function handleSubmit(e: React.FormEvent) {
    e.preventDefault();

    console.log("Order submitted:", { form, cartItems });

    // fake success notification
    localStorage.setItem("orderSuccess", "true");

    navigate("/");
  }

  return (
    <div className="max-w-6xl mx-auto">
      <h1 className="text-4xl font-bold text-gray-800 mb-8">Checkout</h1>

      <div className="grid md:grid-cols-3 gap-8">
        {/* FORM */}
        <form
          onSubmit={handleSubmit}
          className="md:col-span-2 bg-white border border-gray-200 rounded-2xl shadow-sm p-6 space-y-6"
        >
          {/* Shipping */}
          <div>
            <h2 className="text-xl font-semibold mb-4 text-gray-800">
              Shipping Information
            </h2>

            <div className="grid gap-4">
              <input
                name="name"
                placeholder="Full Name"
                onChange={handleChange}
                required
                className="input"
              />
              <input
                name="address"
                placeholder="Address"
                onChange={handleChange}
                required
                className="input"
              />

              <div className="grid grid-cols-3 gap-4">
                <input
                  name="city"
                  placeholder="City"
                  onChange={handleChange}
                  required
                  className="input"
                />
                <input
                  name="state"
                  placeholder="State"
                  onChange={handleChange}
                  required
                  className="input"
                />
                <input
                  name="zip"
                  placeholder="ZIP"
                  onChange={handleChange}
                  required
                  className="input"
                />
              </div>
            </div>
          </div>

          {/* Billing */}
          <div>
            <h2 className="text-xl font-semibold mb-4 text-gray-800">
              Payment Details
            </h2>

            <div className="grid gap-4">
              <input
                name="cardNumber"
                placeholder="Card Number"
                onChange={handleChange}
                required
                className="input"
              />

              <div className="grid grid-cols-2 gap-4">
                <input
                  name="expiration"
                  placeholder="MM/YY"
                  onChange={handleChange}
                  required
                  className="input"
                />
                <input
                  name="cvv"
                  placeholder="CVV"
                  onChange={handleChange}
                  required
                  className="input"
                />
              </div>
            </div>
          </div>

          <button
            type="submit"
            className="w-full py-3 rounded-md bg-blue-600 text-white font-medium hover:bg-blue-700 transition"
          >
            Place Order
          </button>
        </form>

        {/* SUMMARY */}
        <div className="bg-white border border-gray-200 rounded-2xl shadow-sm p-6 h-fit">
          <h2 className="text-2xl font-semibold mb-6 text-gray-800">
            Order Summary
          </h2>

          <div className="space-y-4 mb-6">
            {cartItems.map((item) => (
              <div key={item.id} className="flex justify-between text-sm">
                <span>
                  {item.name} × {item.quantity}
                </span>
                <span>${(item.price * item.quantity).toFixed(2)}</span>
              </div>
            ))}
          </div>

          <div className="flex justify-between mb-2 text-gray-600">
            <span>Subtotal</span>
            <span>${subtotal.toFixed(2)}</span>
          </div>

          <div className="flex justify-between mb-2 text-gray-600">
            <span>Shipping</span>
            <span>Free</span>
          </div>

          <div className="flex justify-between text-xl font-bold">
            <span>Total</span>
            <span>${subtotal.toFixed(2)}</span>
          </div>
        </div>
      </div>
    </div>
  );
}
