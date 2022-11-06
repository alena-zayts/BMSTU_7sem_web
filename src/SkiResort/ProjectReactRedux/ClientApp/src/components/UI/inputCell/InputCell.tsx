import React, { ReactNode } from 'react';
import classes from './InputCell.module.css';
import * as Styles from '../../../styles/components'
import PropTypes, { InferProps } from "prop-types";

const InputCell: any = ({ children, ...props }: InferProps<typeof InputCell.propTypes>) => {
    return (
        <label className={classes.input_cell}>
            <Styles.UsualText className={classes.cell} > {props.whatToInput} </Styles.UsualText>
            <input className={classes.input_cell_text} type={props.type} value={props.value} onChange={props.onChange} />
        </label>
    );
};

InputCell.propTypes = {
    children: PropTypes.oneOfType([
        PropTypes.arrayOf(PropTypes.node),
        PropTypes.node
    ]),

     props: PropTypes.shape (
        {
            whatToInput: PropTypes.string.isRequired,
            value: PropTypes.object.isRequired,
            onChange: PropTypes.func.isRequired,
            type: PropTypes.string.isRequired,
        }
    )
};


export default InputCell;