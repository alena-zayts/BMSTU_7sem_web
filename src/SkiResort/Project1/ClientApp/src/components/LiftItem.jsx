import React from 'react';
import ScreenButton from './UI/button/ScreenButton';

const LiftItem = (props) => {
    return (
        <div className="lift">
            <div className="lift__content">
                <strong>{props.lift.liftName}</strong>
                <div>
                    {props.lift.isOpen}
                </div>
                <div>
                    {props.lift.seatsAmount}
                </div>
                <div>
                    {props.lift.liftingTime}
                </div>
                <div>
                    {props.lift.queueTime}
                </div>
            </div>
            <div className="lift__btns">
                <ScreenButton onClick={() => console.log("b1")}>
                    Удалить
                </ScreenButton>
                <ScreenButton onClick={() => console.log("b2")}>
                    Изменить
                </ScreenButton>
            </div>
        </div>
    );
};

export default LiftItem;


