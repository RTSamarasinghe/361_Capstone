
import api from "../api";
import { useState } from "react";
import { useNavigate } from "react-router-dom";
import { mockProducts } from "../temp/mockProducts";
import ErrorNotification from "../components/Error";
import { getCustomerId } from "../utils/auth/auth"

export default function Checkout() {
  const navigate = useNavigate();
    const [error, setError] = useState<string | null>(null);
    const customerId = getCustomerId();
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

  /**
   * Retrieves customerId from localStorage and parses in to the CheckoutrequestDTOS and AddressDTOs
   * Automatically creates an address linked to customer since it wasn't required to windowshop the site :)
   * @param e
   */
  async function handleSubmit(e: React.FormEvent) {
      e.preventDefault();
      setError(null);

      if (!customerId) {
          setError("You must be logged in to checkout.");
          return;
      }

      try {

          /*Creating address at time of checkoit */
          const addressRes = await api.post("/addresses", {
              customerId,
              street: form.address,
              city: form.city,
              state: form.state,
              postalCode: form.zip,
              country: "USA"
          });

          const addressId = addressRes.data.id;

          const res = await api.post("/orders", {
              customerId,
              totalAmount: subtotal,
              shippingAddress: addressId,
              billingAddressId: addressId

          });

          console.log("Order response:", res.data);
          console.log("Id:", getCustomerId());
          localStorage.setItem("orderSuccess", "true");
          navigate("/");
      } catch (err) {
          setError("Failed to place order. Please try again.");
      }
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
                  {error && <ErrorNotification message={error} />}
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
