import "@/assets/main.css";

const App = () => {
  const colors = ["blue", "red", "green", "yellow"];

  return (
    <div className="content flex justify-center align-center">
      <h1>Rsbuild with React</h1>
      <p>Start building amazing things with Rsbuild.</p>
      {colors.map(color => (
        <button className={`btn btn-${color} mt-3`} key={color}>Click</button>
      ))}
      
      {colors.map(color => (
        <button className={`btn btn-${color}-outline mt-3`} key={color}>Click</button>
      ))}
    </div>
  );
};

export default App;
