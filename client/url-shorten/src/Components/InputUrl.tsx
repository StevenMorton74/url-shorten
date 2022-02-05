import { Button, Input } from 'antd';
import 'antd/dist/antd.min.css';
import { ChangeEvent } from 'react';

interface InputUrlProps {
    handleChange(e: ChangeEvent<HTMLInputElement>): void,
    handleSubmit(): void,
}

function InputUrl(props: InputUrlProps) {
  const { handleChange, handleSubmit } = props;

  return (
    <div className="input-group">
      <Input.Group compact>
        <Input style={{ width: '30vw' }} placeholder="Shorten your url..." onChange={handleChange} onPressEnter={handleSubmit} />
        <Button type="primary" onClick={handleSubmit}>Submit</Button>
      </Input.Group>
    </div>
  );
}

export default InputUrl;
